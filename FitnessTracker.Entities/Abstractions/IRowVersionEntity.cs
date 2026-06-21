using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FitnessTracker.Entities.Abstractions
{
    public interface IRowVersionEntity
    {
        [Timestamp]
        public uint RowVersion { get; set; }
    }
}
