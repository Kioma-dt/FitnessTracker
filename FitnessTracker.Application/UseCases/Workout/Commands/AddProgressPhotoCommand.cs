using FitnessTracker.Application.Interfaces.Images;
using FitnessTracker.Application.Interfaces.Repositories;
using FitnessTracker.Shared.Exceptions.NotFound;
using FitnessTracker.Shared.Exceptions.UnpocessableContent;

namespace FitnessTracker.Application.UseCases.Workout.Commands
{
    public record AddProgressPhotoCommand
    (
        string WorkoutId,
        Stream ImageStream
    )
        : IRequest;

    public class AddProgressPhotoCommandHandler
        : IRequestHandler<AddProgressPhotoCommand>
    {
        IImageRemoteStorage _photosRemoteStorage;
        IStreamImageChecker _streamImageChecker;
        IWorkoutsRepository _workoutsRepository;

        public AddProgressPhotoCommandHandler(
            IImageRemoteStorage photosRemoteStorage,
            IStreamImageChecker streamImageChecker,
            IWorkoutsRepository workoutsRepository)
        {
            _photosRemoteStorage = photosRemoteStorage;
            _streamImageChecker = streamImageChecker;
            _workoutsRepository = workoutsRepository;
        }

        public async Task Handle(
            AddProgressPhotoCommand request,
            CancellationToken cancellationToken)
        {
            var workout = await _workoutsRepository.GetByIdAsync(request.WorkoutId);

            if (workout is null)
            {
                throw new EntityNotFoundException($"No workout with id: {request.WorkoutId}");
            }

            await using var imageBuffer = new MemoryStream();

            await request.ImageStream.CopyToAsync(imageBuffer, cancellationToken);

            var imageBytes = imageBuffer.ToArray();


            await using (var checkImageStreamBuffer = new MemoryStream(imageBytes))
            {
                if (!(await _streamImageChecker.IsSteamImage(checkImageStreamBuffer)))
                {
                    throw new UnprocessableImageException("Erros occuried while decoding image");
                }
            }

            await using (var uploadImageStreamBuffer = new MemoryStream(imageBytes))
            {
                var url = await _photosRemoteStorage.Upload(uploadImageStreamBuffer);

                await _workoutsRepository.AddPhotoAsync(request.WorkoutId, url);
            }
        }
    }
}
