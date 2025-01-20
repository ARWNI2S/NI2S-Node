using ARWNI2S.Engine.Object;

namespace ARWNI2S.Runtime.Simulation.Update
{
    /// <summary>
    /// This is small structure to hold prerequisite tick functions
    /// </summary>
    internal class UpdatePrerequisite
    {
        private readonly NI2SObject _prerequisiteObject;
        private readonly UpdateTask _prerequisiteFunction;

        /// <summary>
        /// Pointer to the actual tick function and must be completed prior to our tick running.
        /// </summary>
        public UpdateTask PrerequisiteFunction
        {
            get
            {
                if (NI2SObject.IsValid(_prerequisiteObject, true))
                {
                    return _prerequisiteFunction;
                }
                return null;
            }
        }

        /// <summary>
        /// Noop constructor.
        /// </summary>
        public UpdatePrerequisite()
        {
            _prerequisiteFunction = null;
        }

        /// <summary> 
        /// Constructor
        /// @param TargetObject - UObject containing this tick function. Only used to verify that the other pointer is still usable
        /// @param TargetTickFunction - Actual tick function to use as a prerequisite
        /// </summary>
        public UpdatePrerequisite(NI2SObject targetObject, UpdateTask targetUpdateFunction)
        {
            ArgumentNullException.ThrowIfNull(targetObject, nameof(targetObject));
            ArgumentNullException.ThrowIfNull(targetUpdateFunction, nameof(targetUpdateFunction));

            _prerequisiteObject = targetObject;
            _prerequisiteFunction = targetUpdateFunction;
        }

        /// <summary>
        /// Equality operator, used to prevent duplicates and allow removal by value.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(UpdatePrerequisite left, UpdatePrerequisite right)
        {
            return left._prerequisiteObject == right._prerequisiteObject &&
                left._prerequisiteFunction == right._prerequisiteFunction;
        }

        public static bool operator !=(UpdatePrerequisite left, UpdatePrerequisite right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return _prerequisiteObject.GetHashCode() & _prerequisiteFunction.GetHashCode();
        }
    }
}
