using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace PostSharp.Tutorials.Threading.Communication
{
    [ServiceContract(Namespace = "Creatures", SessionMode = SessionMode.Required,
                 CallbackContract = typeof(IBoardCallback))]
    public interface IBoardService
    {
        [OperationContract(IsOneWay = true)]
        void MoveCreatureTo(Guid id, double x, double y);

        [OperationContract(IsOneWay = true)]
        void RotateCreatureTo(Guid id, double angle);

        [OperationContract(IsOneWay = true)]
        void CreateCreature(Creature creature);

        [OperationContract]
        List<Creature> GetCreatures();
    }
}
