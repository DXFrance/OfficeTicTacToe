using System.Runtime.Serialization;
using OfficeTicTacToe.Actors.Interfaces;

namespace OfficeTicTacToe.Actors
{
    [DataContract]
    public class TicTacToeReward:IReward
    {
        [DataMember]
        public double Value { get; set; }
        [DataMember]
        public double Discount { get; set; }
        [DataMember]
        public bool IsAbsorbent { get; set; }
        [DataMember]
        public int StateToken { get; set; }
    }
}