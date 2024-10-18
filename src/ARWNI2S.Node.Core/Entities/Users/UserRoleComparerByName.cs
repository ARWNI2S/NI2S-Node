namespace ARWNI2S.Node.Core.Entities.Users
{
    /// <summary>
    /// Custom comparer for the UserRole class by Name and SystemName
    /// </summary>
    public partial class UserRoleComparerByName : IEqualityComparer<UserRole>
    {
        /// <summary>
        /// User roles are equal if their names and system names are equal.
        /// </summary>
        public bool Equals(UserRole x, UserRole y)
        {
            //Check whether the compared objects reference the same data.
            if (ReferenceEquals(x, y))
                return true;

            //Check whether any of the compared objects is null.
            if (x is null || y is null)
                return false;

            //Check whether the user role properties are equal.
            return x.Name == y.Name && x.SystemName == y.SystemName;
        }

        public int GetHashCode(UserRole userRole)
        {
            //Check whether the object is null
            if (userRole is null)
                return 0;

            //Get hash code for the Name field if it is not null.
            var hashUserRoleName = userRole.Name == null ? 0 : userRole.Name.GetHashCode();

            //Get hash code for the SystemName field.
            var hashUserRoleSystemName = userRole.SystemName == null ? 0 : userRole.SystemName.GetHashCode();

            //Calculate the hash code for the UserRole.
            return hashUserRoleName ^ hashUserRoleSystemName;
        }
    }
}
