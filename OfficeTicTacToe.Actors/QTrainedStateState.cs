using System.Collections.Generic;
using System.Runtime.Serialization;

namespace OfficeTicTacToe.Actors
{
 [DataContract]
 public class QTrainedStateState
 {
     [DataMember]
     public double MaximumReward { get; set; }

     [DataMember]
     public HashSet<int> ChildrenQTrainedStates { get; set; }
 }
}