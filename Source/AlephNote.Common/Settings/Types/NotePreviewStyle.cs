﻿namespace AlephNote.Common.Settings.Types
{
	public enum NotePreviewStyle
	{
		[EnumDescriptor("Simple one line")]
		Simple,

		[EnumDescriptor("One line with date")]
		Extended,

		[EnumDescriptor("Multiple lines with preview")]
		FullPreview,
	}
}