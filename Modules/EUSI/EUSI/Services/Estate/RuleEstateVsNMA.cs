using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Extensions;

namespace EUSI.Services.Estate
{


    public struct StateTraverse<TStateFrom, TStateTo>
    {
        public TStateFrom From;
        public TStateTo To;
    }

    /// <summary>
    /// Переход
    /// </summary>
    /// <typeparam name="TAction">Действия по переходу</typeparam>
    /// <typeparam name="TStateKey">Тип уникального поля</typeparam>
    /// <typeparam name="TState">Состояние</typeparam>
    /// <typeparam name="TObject"></typeparam>
    public class Transtiton<TState, TStateKey, TObject>
        where TState : IComparable<TStateKey>, IEquatable<TStateKey>
    {
        public TState From { get; set; }
        public TState To { get; set; }
        private Predicate<TObject> Condition { get; set; }
    }




    

    public class TransitionMapEstateRegistration
    {
        
        //public Transtiton<string, string, string> trans;

    }

    /// <summary>
    /// Конечный автомат
    /// </summary>
    public class FSM<TState, TAction, TStateKey, TObject>
        where TState : IComparable<TStateKey>, IEquatable<TStateKey>
    {
        private LinkedListNode<TState> FsmNodes { get; set; }

        void CreateFsmNodesFromTransition(List<Transtiton<TState, TStateKey, TObject>> transitions)
        {
            throw new NotImplementedException();
        }



        public FSM(List<Transtiton<TState, TStateKey, TObject>> transitions)
        {
            CreateFsmNodesFromTransition(transitions);
        }

        public bool TryMoveTo(TState From, TState To)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TState> GetNextStates()
        {
            throw new NotImplementedException();
        }


    }

}
