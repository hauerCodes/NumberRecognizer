//------------------------------------------------------------------------------
// <auto-generated>
//     Der Code wurde von einer Vorlage generiert.
//
//     Manuelle Änderungen an dieser Datei führen möglicherweise zu unerwartetem Verhalten der Anwendung.
//     Manuelle Änderungen an dieser Datei werden überschrieben, wenn der Code neu generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NumberRecognizer.Cloud.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Network
    {
        public Network()
        {
            this.TrainingImages = new HashSet<TrainingImageData>();
            this.TrainLogs = new HashSet<TrainLog>();
        }
    
        public int NetworkId { get; set; }
        public string NetworkName { get; set; }
        public CalculationType Calculated { get; set; }
        public double Fitness { get; set; }
        public byte[] NetworkData { get; set; }
        public Nullable<System.DateTime> CalculationStart { get; set; }
        public Nullable<System.DateTime> CalculationEnd { get; set; }
        public string ErrorStatus { get; set; }
    
        public virtual ICollection<TrainingImageData> TrainingImages { get; set; }
        public virtual ICollection<TrainLog> TrainLogs { get; set; }
    }
}
