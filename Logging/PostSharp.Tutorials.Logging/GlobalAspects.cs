using PostSharp.Patterns.Diagnostics;

[assembly: Log(AttributePriority = 1)]
[assembly: Log(AttributePriority = 2, AttributeExclude = true, AttributeTargetMembers = "regex:get_*|set_*")]