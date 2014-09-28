using System;

namespace DistantLearningSystem.Models.LogicModels.Managers
{
    [Flags]
    public enum UserStatus
    {
        Unconfirmed = 1,
        Confirmed = 2
    }
}