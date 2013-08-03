using CruiseControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CruiseControl
{
    public class Commander
    {
        public BoardStatus _currentBoard;

        private Random _randomGenerator;

        public Commander()
        {
            _currentBoard = new BoardStatus();
            _randomGenerator = new Random();
        }

        private bool TooCloseToEdge(VesselStatus vessel)
        {
            return vessel.Location.Any(c => c.X == _currentBoard.BoardMinCoordinate.X + 1 || 
                                            c.Y == _currentBoard.BoardMinCoordinate.Y + 1 || 
                                            c.X == _currentBoard.BoardMaxCoordinate.X - 1 || 
                                            c.Y == _currentBoard.BoardMaxCoordinate.Y - 1);
        }

        private string GetOppositeDirectionFromClosestEdge(VesselStatus vessel)
        {
            if (vessel.Location.Any(c => c.X == _currentBoard.BoardMinCoordinate.X + 1))
                return "east";
            if (vessel.Location.Any(c => c.Y == _currentBoard.BoardMinCoordinate.Y + 1))
                return "south";
            if (vessel.Location.Any(c => c.X == _currentBoard.BoardMaxCoordinate.X - 1))
                return "west";
            return "east";
        }

        private string GetRandomMovementDirection()
        {
            var number = _randomGenerator.Next(4);
            switch (number)
            {
                case 0:
                    return "east";
                case 1:
                    return "west";
                case 2:
                    return "north";
                default:
                    return "south";
            }
        }

        // Do not alter/remove this method signature
        public List<Command> GiveCommands()
        {
            var cmds = new List<Command>();

            // Add Commands Here.
            // You can only give as many commands as you have un-sunk vessels. Powerup commands do not count against this number. 
            // You are free to use as many powerup commands at any time. Any additional commands you give (past the number of active vessels) will be ignored.

            foreach (var vessel in _currentBoard.MyVesselStatuses)
            {
                if (vessel.SonarReport.Any())
                    cmds.Add(new Command { vesselid = vessel.Id, action = "fire", coordinate = new Coordinate { X = vessel.SonarReport[0].X, Y = vessel.SonarReport[0].Y } });
                else if (TooCloseToEdge(vessel))
                    cmds.Add(new Command { vesselid = vessel.Id, action = "move:" + GetOppositeDirectionFromClosestEdge(vessel) });
                else
                    cmds.Add(new Command { vesselid = vessel.Id, action = "move:" + GetRandomMovementDirection() });
            }

            return cmds;
        }

        // Do NOT modify or remove! This is where you will receive the new board status after each round.
        public void GetBoardStatus(BoardStatus board)
        {
            _currentBoard = board;
        }

        // This method runs at the start of a new game, do any initialization or resetting here 
        public void Reset()
        {

        }
    }
}