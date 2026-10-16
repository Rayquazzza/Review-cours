using System;
using System.Collections.Generic;
using UnityEngine;
namespace NavMeshPatrol.Guard
{
    public sealed class StateMachine
    {
        private readonly string owner;
        private readonly Dictionary<IState, List<Transition>> transitions = new Dictionary<IState, List<Transition>>();
        private readonly List<Transition> anyTransitions = new List<Transition>();
        private IState suspended;
        private readonly List<Interruption> interruptions = new List<Interruption>();
        private Interruption active;

        public StateMachine(string owner)
        {
            this.owner = owner;
        }

        public IState Current { get; private set; }

        public bool IsInterrupted => suspended != null;

        // Declares a state that can pause any other one. The paused state is suspended, not left: no
        // Exit when the interruption starts, no Enter when it ends, so its progress survives.
        public void AddInterruption(IState state, Func<bool> starts, Func<bool> ends)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            if (starts == null) throw new ArgumentNullException(nameof(starts));
            if (ends == null) throw new ArgumentNullException(nameof(ends));
            interruptions.Add(new Interruption(state, starts, ends));
        }

        public void AddTransition(IState from, IState to, Func<bool> condition)
        {
            if (!transitions.TryGetValue(from, out List<Transition> list))
            {
                list = new List<Transition>();
                transitions.Add(from, list);
            }
            list.Add(new Transition(to, condition));
        }

        // Checked before the transitions of the current state, from whatever state the machine is in.
        public void AddAnyTransition(IState to, Func<bool> condition)
        {
            anyTransitions.Add(new Transition(to, condition));
        }

        // Exit before Enter: the old state cleans up before the new one sets itself up.
        public void ChangeState(IState next)
        {
            if (next == null) throw new ArgumentNullException(nameof(next));
            Current?.Exit();
            Debug.Log($"{owner}: {NameOf(Current)} -> {NameOf(next)}");
            Current = next;
            Current.Enter();
        }

        // At most one change per frame, then the current state runs. While interrupted, the ordinary
        // transitions are frozen: only the end of the interruption is watched.
        public void Tick(float deltaTime)
        {
            if (IsInterrupted)
            {
                if (active.Ends()) Resume();
            }
            else if (FindInterruption() is Interruption starting)
            {
                Suspend(starting);
            }
            else
            {
                Transition transition = FindTransition();
                if (transition != null) ChangeState(transition.To);
            }
            Current?.Tick(deltaTime);
        }

        private void Suspend(Interruption interruption)
        {
            suspended = Current;
            active = interruption;
            Debug.Log($"{owner}: {NameOf(suspended)} suspended -> {NameOf(interruption.State)}");
            Current = interruption.State;
            Current.Enter();
        }

        private void Resume()
        {
            Current.Exit();
            Debug.Log($"{owner}: {NameOf(active.State)} -> {NameOf(suspended)} resumed");
            Current = suspended;
            suspended = null;
            active = null;
            (Current as IResumable)?.Resume();
        }



        private Interruption FindInterruption()
        {
            foreach (Interruption interruption in interruptions)
            {
                if (interruption.Starts()) return interruption;
            }
            return null;
        }


        private Transition FindTransition()
        {
            foreach (Transition any in anyTransitions)
            {
                // Without this check, an "any" transition re-enters its own target every frame.
                if (any.To != Current && any.Condition()) return any;
            }
            if (Current == null || !transitions.TryGetValue(Current, out List<Transition> list)) return null;
            foreach (Transition transition in list)
            {
                if (transition.Condition()) return transition;
            }
            return null;
        }

        private static string NameOf(IState state) => state == null ? "none" : state.GetType().Name;


        private class Interruption
        {
            public Interruption(IState state, Func<bool> starts, Func<bool> ends)
            {
                State = state;
                Starts = starts;
                Ends = ends;
            }

            public IState State { get; }

            public Func<bool> Starts { get; }

            public Func<bool> Ends { get; }
        }
        private sealed class Transition
        {
            public Transition(IState to, Func<bool> condition)
            {
                To = to;
                Condition = condition;
            }

            public IState To { get; }

            public Func<bool> Condition { get; }
        }
    }
}
