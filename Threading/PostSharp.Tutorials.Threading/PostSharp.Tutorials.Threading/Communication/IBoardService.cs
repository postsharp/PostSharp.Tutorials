using System;
using System.Collections.Generic;
using System.ServiceModel;
using PostSharp.Tutorials.Threading.Model;

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

        [OperationContract(IsOneWay = true)]
        void DeleteCreature(Guid id);

        [OperationContract]
        List<Creature> GetCreatures();
    }
}
