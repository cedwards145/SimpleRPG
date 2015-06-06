using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRPG
{
    public class MoveRoute
    {
        private Queue<Moves> moves;
        private bool repeating = false;
        private bool reverse = false;

        public MoveRoute(ICollection<Moves> newMoves)
            :this(newMoves, false, false)
        {  }

        public MoveRoute(ICollection<Moves> newMoves, bool isRepeating, bool isReverse)
        {
            repeating = isRepeating;
            reverse = isReverse;
            moves = new Queue<Moves>(newMoves);

            if (isReverse)
                for (int index = newMoves.Count - 1; index >= 0; index--)
                    moves.Enqueue(newMoves.ElementAt(index));
        }

        public void setRoute(ICollection<Moves> newMoves)
        {
            moves = new Queue<Moves>(newMoves);
        }

        public Moves dequeue()
        {
            Moves returnValue = Moves.None;
            if (moves.Count > 0)
            {
                returnValue = moves.Dequeue();
                if (repeating)
                    moves.Enqueue(returnValue);
            }
            return returnValue;
        }

        public Moves peek()
        {
            Moves returnValue = Moves.None;
            if (moves.Count > 0)
                returnValue = moves.Peek();

            return returnValue;
        }

        public bool isCompleted()
        {
            return moves.Count <= 0;
        }
    }
}
