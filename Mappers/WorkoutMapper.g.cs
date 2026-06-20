using System;
using System.Collections.Generic;
using System.Linq;
using FitnessTracker.Application.Mappers;
using FitnessTracker.Entities;
using FitnessTracker.Shared.DTO.Responses;
using FitnessTracker.Shared.Enums;

namespace FitnessTracker.Application.Mappers
{
    public partial class WorkoutMapper : IWorkoutMapper
    {
        public WorkoutResponseDTO MapTo(Workout p1)
        {
            if (p1 == null)
            {
                return null;
            }
            WorkoutResponseDTO result = new WorkoutResponseDTO(p1 != null && p1.Id != null ? p1.Id : default(string), p1 != null && p1.Title != null ? p1.Title : default(string), p1 != null ? p1.Type : default(WorkoutType), p1 != null ? (int)p1.Duration.TotalMinutes : default(int), p1 != null ? p1.CaloriesBurned : default(int), p1 != null ? p1.WorkoutDate : default(DateTime), p1 != null && p1.Exercises != null ? funcMain1(p1.Exercises) : default(List<ExerciseResponseDTO>), p1 != null && p1.ProgressPhotos != null ? funcMain5(p1.ProgressPhotos) : default(List<string>)) {DurationInMinutes = (int)p1.Duration.TotalMinutes};
            return result;
            
        }
        public IEnumerable<WorkoutResponseDTO> MapTo(IEnumerable<Workout> p7)
        {
            return p7 == null ? null : p7.Select<Workout, WorkoutResponseDTO>(funcMain6);
        }
        public ExerciseResponseDTO MapTo(Exercise p15)
        {
            if (p15 == null)
            {
                return null;
            }
            ExerciseResponseDTO result = new ExerciseResponseDTO(p15 != null && p15.Name != null ? p15.Name : default(string), p15 != null && p15.Sets != null ? funcMain13(p15.Sets) : default(List<SetResponseDTO>)) {};
            return result;
            
        }
        
        private List<ExerciseResponseDTO> funcMain1(List<Exercise> p2)
        {
            if (p2 == null)
            {
                return null;
            }
            List<ExerciseResponseDTO> result = new List<ExerciseResponseDTO>(p2.Count);
            
            int i = 0;
            int len = p2.Count;
            
            while (i < len)
            {
                Exercise item = p2[i];
                result.Add(funcMain2(item));
                i++;
            }
            return result;
            
        }
        
        private List<string> funcMain5(List<string> p6)
        {
            if (p6 == null)
            {
                return null;
            }
            List<string> result = new List<string>(p6.Count);
            
            int i = 0;
            int len = p6.Count;
            
            while (i < len)
            {
                string item = p6[i];
                result.Add(item);
                i++;
            }
            return result;
            
        }
        
        private WorkoutResponseDTO funcMain6(Workout p8)
        {
            return funcMain7(p8);
        }
        
        private List<SetResponseDTO> funcMain13(List<Set> p16)
        {
            if (p16 == null)
            {
                return null;
            }
            List<SetResponseDTO> result = new List<SetResponseDTO>(p16.Count);
            
            int i = 0;
            int len = p16.Count;
            
            while (i < len)
            {
                Set item = p16[i];
                result.Add(funcMain14(item));
                i++;
            }
            return result;
            
        }
        
        private ExerciseResponseDTO funcMain2(Exercise p3)
        {
            if (p3 == null)
            {
                return null;
            }
            ExerciseResponseDTO result = new ExerciseResponseDTO(p3 != null && p3.Name != null ? p3.Name : default(string), p3 != null && p3.Sets != null ? funcMain3(p3.Sets) : default(List<SetResponseDTO>)) {};
            return result;
            
        }
        
        private WorkoutResponseDTO funcMain7(Workout p9)
        {
            if (p9 == null)
            {
                return null;
            }
            WorkoutResponseDTO result = new WorkoutResponseDTO(p9 != null && p9.Id != null ? p9.Id : default(string), p9 != null && p9.Title != null ? p9.Title : default(string), p9 != null ? p9.Type : default(WorkoutType), p9 != null ? (int)p9.Duration.TotalMinutes : default(int), p9 != null ? p9.CaloriesBurned : default(int), p9 != null ? p9.WorkoutDate : default(DateTime), p9 != null && p9.Exercises != null ? funcMain8(p9.Exercises) : default(List<ExerciseResponseDTO>), p9 != null && p9.ProgressPhotos != null ? funcMain12(p9.ProgressPhotos) : default(List<string>)) {DurationInMinutes = (int)p9.Duration.TotalMinutes};
            return result;
            
        }
        
        private SetResponseDTO funcMain14(Set p17)
        {
            if (p17 == null)
            {
                return null;
            }
            SetResponseDTO result = new SetResponseDTO(p17 != null ? p17.Weight : default(double), p17 != null ? p17.Reps : default(int))
            {
                Weight = p17.Weight,
                Reps = p17.Reps
            };
            return result;
            
        }
        
        private List<SetResponseDTO> funcMain3(List<Set> p4)
        {
            if (p4 == null)
            {
                return null;
            }
            List<SetResponseDTO> result = new List<SetResponseDTO>(p4.Count);
            
            int i = 0;
            int len = p4.Count;
            
            while (i < len)
            {
                Set item = p4[i];
                result.Add(funcMain4(item));
                i++;
            }
            return result;
            
        }
        
        private List<ExerciseResponseDTO> funcMain8(List<Exercise> p10)
        {
            if (p10 == null)
            {
                return null;
            }
            List<ExerciseResponseDTO> result = new List<ExerciseResponseDTO>(p10.Count);
            
            int i = 0;
            int len = p10.Count;
            
            while (i < len)
            {
                Exercise item = p10[i];
                result.Add(funcMain9(item));
                i++;
            }
            return result;
            
        }
        
        private List<string> funcMain12(List<string> p14)
        {
            if (p14 == null)
            {
                return null;
            }
            List<string> result = new List<string>(p14.Count);
            
            int i = 0;
            int len = p14.Count;
            
            while (i < len)
            {
                string item = p14[i];
                result.Add(item);
                i++;
            }
            return result;
            
        }
        
        private SetResponseDTO funcMain4(Set p5)
        {
            if (p5 == null)
            {
                return null;
            }
            SetResponseDTO result = new SetResponseDTO(p5 != null ? p5.Weight : default(double), p5 != null ? p5.Reps : default(int))
            {
                Weight = p5.Weight,
                Reps = p5.Reps
            };
            return result;
            
        }
        
        private ExerciseResponseDTO funcMain9(Exercise p11)
        {
            if (p11 == null)
            {
                return null;
            }
            ExerciseResponseDTO result = new ExerciseResponseDTO(p11 != null && p11.Name != null ? p11.Name : default(string), p11 != null && p11.Sets != null ? funcMain10(p11.Sets) : default(List<SetResponseDTO>)) {};
            return result;
            
        }
        
        private List<SetResponseDTO> funcMain10(List<Set> p12)
        {
            if (p12 == null)
            {
                return null;
            }
            List<SetResponseDTO> result = new List<SetResponseDTO>(p12.Count);
            
            int i = 0;
            int len = p12.Count;
            
            while (i < len)
            {
                Set item = p12[i];
                result.Add(funcMain11(item));
                i++;
            }
            return result;
            
        }
        
        private SetResponseDTO funcMain11(Set p13)
        {
            if (p13 == null)
            {
                return null;
            }
            SetResponseDTO result = new SetResponseDTO(p13 != null ? p13.Weight : default(double), p13 != null ? p13.Reps : default(int))
            {
                Weight = p13.Weight,
                Reps = p13.Reps
            };
            return result;
            
        }
    }
}