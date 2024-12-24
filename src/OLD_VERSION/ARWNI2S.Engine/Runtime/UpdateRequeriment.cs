using ARWNI2S.Engine.Core.Extensions;
using ARWNI2S.Engine.Core.Object;

namespace ARWNI2S.Engine.Runtime
{
	public class UpdateRequeriment
	{
		/** Tick functions live inside of UObjects, so we need a separate weak pointer to the UObject solely for the purpose of determining if PrerequisiteTickFunction is still valid. */
		INiisObject _requerimentObject;

		/** Pointer to the actual tick function and must be completed prior to our tick running. */
		UpdateFunction _updateRequerimentFunction;

		/** Noop constructor. */
		UpdateRequeriment()
		{
			_updateRequerimentFunction = null;
		}
		/** 
			* Constructor
			* @param TargetObject - UObject containing this tick function. Only used to verify that the other pointer is still usable
			* @param TargetTickFunction - Actual tick function to use as a prerequisite
		**/
		UpdateRequeriment(INiisObject targetObject, UpdateFunction targetUpdateFunction)
		{
			ArgumentNullException.ThrowIfNull(targetObject, nameof(targetObject));
			ArgumentNullException.ThrowIfNull(targetUpdateFunction, nameof(targetUpdateFunction));

			_requerimentObject = targetObject;
			_updateRequerimentFunction = targetUpdateFunction;
		}
		/** Equality operator, used to prevent duplicates and allow removal by value. */
		public static bool operator ==(UpdateRequeriment left, UpdateRequeriment right)
		{
			return left._requerimentObject == right._requerimentObject &&
				left._updateRequerimentFunction == right._updateRequerimentFunction;
		}

		public static bool operator !=(UpdateRequeriment left, UpdateRequeriment right)
		{
			return left._requerimentObject != right._requerimentObject ||
				left._updateRequerimentFunction != right._updateRequerimentFunction;
		}

		/** Return the tick function, if it is still valid. Can be null if the tick function was null or the containing UObject has been garbage collected. */
		UpdateFunction Get()
		{
			if (_requerimentObject.IsValid(true))
			{
				return _updateRequerimentFunction;
			}
			return null;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(this, obj))
			{
				return true;
			}

			if (ReferenceEquals(obj, null))
			{
				return false;
			}

			throw new NotImplementedException();
		}

		public override int GetHashCode()
		{
			throw new NotImplementedException();
		}
	}

}