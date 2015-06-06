using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRPG
{
    /// <summary>
    /// Enumeration of all the buttons used in the game. In the controller class, 
    /// keyboard keys and 360 Pad buttons are mapped to each of these buttons
    /// </summary>
    public enum ControllerButton { up, down, left, right, enter, back }

    /// <summary>
    /// Used to represent the four directions of movement on tilemaps
    /// </summary>
    public enum Facing { Up, Down, Left, Right, None };

    public enum Moves { MoveUp, MoveDown, MoveLeft, MoveRight, FaceUp, FaceDown, FaceLeft, FaceRight, None };

    /// <summary>
    /// Used by Tiles and MapObjects to determine whether objects can pass through them.
    /// Ignore - Takes the passability from the tile below it
    /// True - Objects can pass through
    /// False - Objects cannot pass through
    /// </summary>
    public enum Passability { True, False, Ignore };

    /// <summary>
    /// Determines what Battlers a Skill or Item are able to target.
    /// Friends - Battlers in the same party as the user
    /// Enemies - Battlers not in the same party as the user
    /// All - Any Battler
    /// </summary>
    public enum ValidTargets { Friends, Enemies, All };

    /// <summary>
    /// Determines whether skills or items cause positive or negative changes
    /// to a targets HP
    /// </summary>
    public enum DamageType { Healing, Harming };

    /// <summary>
    /// Used to specify which direction text should align
    /// </summary>
    public enum TextAlign { Center, Left, Right };

}

namespace SimpleRPG.Crafting
{
    /// <summary>
    /// Used to specify which crafting action will be performed
    /// </summary>
    public enum CraftAction { Chop, Cook, Crush, Brew };
}

namespace SimpleRPG.Scripts
{
    public enum ScriptBehaviour { Default, NonBlocking };
    public enum ScriptActivationType { Action, Collision, Load };
}