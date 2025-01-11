// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

namespace ARWNI2S.Presentation
{
    /// <summary>
    /// Represents an attribute control type
    /// </summary>
    public enum AttributeControlType
    {
        /// <summary>
        /// Dropdown list
        /// </summary>
        DropdownList = 1,

        /// <summary>
        /// Radio list
        /// </summary>
        RadioList = 2,

        /// <summary>
        /// Checkboxes
        /// </summary>
        Checkboxes = 3,

        /// <summary>
        /// TextBox
        /// </summary>
        TextBox = 4,

        /// <summary>
        /// Multiline textbox
        /// </summary>
        MultilineTextbox = 10,

        /// <summary>
        /// Datepicker
        /// </summary>
        Datepicker = 20,

        /// <summary>
        /// File upload control
        /// </summary>
        FileUpload = 30,

        /// <summary>
        /// Color squares
        /// </summary>
        ColorSquares = 40,

        /// <summary>
        /// Image squares
        /// </summary>
        ImageSquares = 45,

        /// <summary>
        /// Read-only checkboxes
        /// </summary>
        ReadonlyCheckboxes = 50
    }
}