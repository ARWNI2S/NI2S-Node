using ARWNI2S.Engine.Object;

namespace ARWNI2S.Runtime.UpdateRing
{
    internal class UpdatePrerequisite
    {
        private WeakReference _prerequisiteObject;

        private UpdateFunction _prerequisiteUpdateFunction;

        public UpdatePrerequisite()
        {
            _prerequisiteUpdateFunction = null;
        }

        public UpdatePrerequisite(NI2SObject targetObject, UpdateFunction targetUpdateFunction)
        {
            ArgumentNullException.ThrowIfNull(targetObject, nameof(targetObject));
            ArgumentNullException.ThrowIfNull(targetUpdateFunction, nameof(targetUpdateFunction));

            _prerequisiteObject = new WeakReference(targetObject);
            _prerequisiteUpdateFunction = targetUpdateFunction;
        }
    }
}
/////**
//// * This is small structure to hold prerequisite tick functions
//// */
////USTRUCT()
////struct FTickPrerequisite
////{
////    GENERATED_USTRUCT_BODY()

////    /** Tick functions live inside of UObjects, so we need a separate weak pointer to the UObject solely for the purpose of determining if PrerequisiteTickFunction is still valid. */
////    TWeakObjectPtr<class UObject> PrerequisiteObject;

////	/** Pointer to the actual tick function and must be completed prior to our tick running. */
////	struct FTickFunction*		PrerequisiteTickFunction;

////	/** Noop constructor. */
////	FTickPrerequisite()
////    : PrerequisiteTickFunction(nullptr)
////    {
////    }
////    /** 
////		* Constructor
////		* @param TargetObject - UObject containing this tick function. Only used to verify that the other pointer is still usable
////		* @param TargetTickFunction - Actual tick function to use as a prerequisite
////	**/
////    FTickPrerequisite(UObject* TargetObject, struct FTickFunction& TargetTickFunction)
////	: PrerequisiteObject(TargetObject)
////	, PrerequisiteTickFunction(&TargetTickFunction)
////    {
////        check(PrerequisiteTickFunction);
////    }
////    /** Equality operator, used to prevent duplicates and allow removal by value. */
////    bool operator ==(const FTickPrerequisite& Other) const
////	{
////		return PrerequisiteObject == Other.PrerequisiteObject &&
////			PrerequisiteTickFunction == Other.PrerequisiteTickFunction;
////	}
////    /** Return the tick function, if it is still valid. Can be null if the tick function was null or the containing UObject has been garbage collected. */
////    struct FTickFunction* Get()
////    {
////        if (PrerequisiteObject.IsValid(true))
////        {
////            return PrerequisiteTickFunction;
////        }
////        return nullptr;
////    }

////    const struct FTickFunction*Get() const
////	{
////		if (PrerequisiteObject.IsValid(true))
////		{
////			return PrerequisiteTickFunction;
////		}
////		return nullptr;
////	}
////};