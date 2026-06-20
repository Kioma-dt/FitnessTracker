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
        public WorkoutResponseDTO MapTo(Workout p7, WorkoutResponseDTO p8)
        {
            if (p7 == null)
            {
                return null;
            }
            WorkoutResponseDTO result = new WorkoutResponseDTO(p7 != null && p7.Id != null ? p7.Id : default(string), p7 != null && p7.Title != null ? p7.Title : default(string), p7 != null ? p7.Type : default(WorkoutType), p7 != null ? (int)p7.Duration.TotalMinutes : default(int), p7 != null ? p7.CaloriesBurned : default(int), p7 != null ? p7.WorkoutDate : default(DateTime), p7 != null && p7.Exercises != null ? funcMain6(p7.Exercises) : default(List<ExerciseResponseDTO>), p7 != null && p7.ProgressPhotos != null ? funcMain10(p7.ProgressPhotos) : default(List<string>)) {DurationInMinutes = (int)p7.Duration.TotalMinutes};
            return result;
            
        }
        public IEnumerable<WorkoutResponseDTO> MapTo(IEnumerable<Workout> p14)
        {
            return p14 == null ? null : p14.Select<Workout, WorkoutResponseDTO>(funcMain11);
        }
        public ExerciseResponseDTO MapTo(Exercise p22)
        {
            if (p22 == null)
            {
                return null;
            }
            ExerciseResponseDTO result = new ExerciseResponseDTO(p22 != null && p22.Name != null ? p22.Name : default(string), p22 != null && p22.Sets != null ? funcMain18(p22.Sets) : default(List<SetResponseDTO>)) {};
            return result;
            
        }
        public ExerciseResponseDTO MapTo(Exercise p25, ExerciseResponseDTO p26)
        {
            if (p25 == null)
            {
                return null;
            }
            ExerciseResponseDTO result = new ExerciseResponseDTO(p25 != null && p25.Name != null ? p25.Name : default(string), p25 != null && p25.Sets != null ? funcMain20(p25.Sets) : default(List<SetResponseDTO>)) {};
            return result;
            
        }
        public SetResponseDTO MapTo(Set p29)
        {
            if (p29 == null)
            {
                return null;
            }
            SetResponseDTO result = new SetResponseDTO(p29 != null ? p29.Weight : default(double), p29 != null ? p29.Reps : default(int)) {};
            return result;
            
        }
        public SetResponseDTO MapTo(Set p30, SetResponseDTO p31)
        {
            if (p30 == null)
            {
                return null;
            }
            SetResponseDTO result = new SetResponseDTO(p30 != null ? p30.Weight : default(double), p30 != null ? p30.Reps : default(int)) {};
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
        
        private List<ExerciseResponseDTO> funcMain6(List<Exercise> p9)
        {
            if (p9 == null)
            {
                return null;
            }
            List<ExerciseResponseDTO> result = new List<ExerciseResponseDTO>(p9.Count);
            
            int i = 0;
            int len = p9.Count;
            
            while (i < len)
            {
                Exercise item = p9[i];
                result.Add(funcMain7(item));
                i++;
            }
            return result;
            
        }
        
        private List<string> funcMain10(List<string> p13)
        {
            if (p13 == null)
            {
                return null;
            }
            List<string> result = new List<string>(p13.Count);
            
            int i = 0;
            int len = p13.Count;
            
            while (i < len)
            {
                string item = p13[i];
                result.Add(item);
                i++;
            }
            return result;
            
        }
        
        private WorkoutResponseDTO funcMain11(Workout p15)
        {
            return funcMain12(p15);
        }
        
        private List<SetResponseDTO> funcMain18(List<Set> p23)
        {
            if (p23 == null)
            {
                return null;
            }
            List<SetResponseDTO> result = new List<SetResponseDTO>(p23.Count);
            
            int i = 0;
            int len = p23.Count;
            
            while (i < len)
            {
                Set item = p23[i];
                result.Add(funcMain19(item));
                i++;
            }
            return result;
            
        }
        
        private List<SetResponseDTO> funcMain20(List<Set> p27)
        {
            if (p27 == null)
            {
                return null;
            }
            List<SetResponseDTO> result = new List<SetResponseDTO>(p27.Count);
            
            int i = 0;
            int len = p27.Count;
            
            while (i < len)
            {
                Set item = p27[i];
                result.Add(funcMain21(item));
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
        
        private ExerciseResponseDTO funcMain7(Exercise p10)
        {
            if (p10 == null)
            {
                return null;
            }
            ExerciseResponseDTO result = new ExerciseResponseDTO(p10 != null && p10.Name != null ? p10.Name : default(string), p10 != null && p10.Sets != null ? funcMain8(p10.Sets) : default(List<SetResponseDTO>)) {};
            return result;
            
        }
        
        private WorkoutResponseDTO funcMain12(Workout p16)
        {
            if (p16 == null)
            {
                return null;
            }
            WorkoutResponseDTO result = new WorkoutResponseDTO(p16 != null && p16.Id != null ? p16.Id : default(string), p16 != null && p16.Title != null ? p16.Title : default(string), p16 != null ? p16.Type : default(WorkoutType), p16 != null ? (int)p16.Duration.TotalMinutes : default(int), p16 != null ? p16.CaloriesBurned : default(int), p16 != null ? p16.WorkoutDate : default(DateTime), p16 != null && p16.Exercises != null ? funcMain13(p16.Exercises) : default(List<ExerciseResponseDTO>), p16 != null && p16.ProgressPhotos != null ? funcMain17(p16.ProgressPhotos) : default(List<string>)) {DurationInMinutes = (int)p16.Duration.TotalMinutes};
            return result;
            
        }
        
        private SetResponseDTO funcMain19(Set p24)
        {
            if (p24 == null)
            {
                return null;
            }
            SetResponseDTO result = new SetResponseDTO(p24 != null ? p24.Weight : default(double), p24 != null ? p24.Reps : default(int)) {};
            return result;
            
        }
        
        private SetResponseDTO funcMain21(Set p28)
        {
            if (p28 == null)
            {
                return null;
            }
            SetResponseDTO result = new SetResponseDTO(p28 != null ? p28.Weight : default(double), p28 != null ? p28.Reps : default(int)) {};
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
        
        private List<SetResponseDTO> funcMain8(List<Set> p11)
        {
            if (p11 == null)
            {
                return null;
            }
            List<SetResponseDTO> result = new List<SetResponseDTO>(p11.Count);
            
            int i = 0;
            int len = p11.Count;
            
            while (i < len)
            {
                Set item = p11[i];
                result.Add(funcMain9(item));
                i++;
            }
            return result;
            
        }
        
        private List<ExerciseResponseDTO> funcMain13(List<Exercise> p17)
        {
            if (p17 == null)
            {
                return null;
            }
            List<ExerciseResponseDTO> result = new List<ExerciseResponseDTO>(p17.Count);
            
            int i = 0;
            int len = p17.Count;
            
            while (i < len)
            {
                Exercise item = p17[i];
                result.Add(funcMain14(item));
                i++;
            }
            return result;
            
        }
        
        private List<string> funcMain17(List<string> p21)
        {
            if (p21 == null)
            {
                return null;
            }
            List<string> result = new List<string>(p21.Count);
            
            int i = 0;
            int len = p21.Count;
            
            while (i < len)
            {
                string item = p21[i];
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
            SetResponseDTO result = new SetResponseDTO(p5 != null ? p5.Weight : default(double), p5 != null ? p5.Reps : default(int)) {};
            return result;
            
        }
        
        private SetResponseDTO funcMain9(Set p12)
        {
            if (p12 == null)
            {
                return null;
            }
            SetResponseDTO result = new SetResponseDTO(p12 != null ? p12.Weight : default(double), p12 != null ? p12.Reps : default(int)) {};
            return result;
            
        }
        
        private ExerciseResponseDTO funcMain14(Exercise p18)
        {
            if (p18 == null)
            {
                return null;
            }
            ExerciseResponseDTO result = new ExerciseResponseDTO(p18 != null && p18.Name != null ? p18.Name : default(string), p18 != null && p18.Sets != null ? funcMain15(p18.Sets) : default(List<SetResponseDTO>)) {};
            return result;
            
        }
        
        private List<SetResponseDTO> funcMain15(List<Set> p19)
        {
            if (p19 == null)
            {
                return null;
            }
            List<SetResponseDTO> result = new List<SetResponseDTO>(p19.Count);
            
            int i = 0;
            int len = p19.Count;
            
            while (i < len)
            {
                Set item = p19[i];
                result.Add(funcMain16(item));
                i++;
            }
            return result;
            
        }
        
        private SetResponseDTO funcMain16(Set p20)
        {
            if (p20 == null)
            {
                return null;
            }
            SetResponseDTO result = new SetResponseDTO(p20 != null ? p20.Weight : default(double), p20 != null ? p20.Reps : default(int)) {};
            return result;
            
        }
    }
}