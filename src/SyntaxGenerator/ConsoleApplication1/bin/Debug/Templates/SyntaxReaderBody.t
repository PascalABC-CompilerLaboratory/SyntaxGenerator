<# set TemplateType = "SyntaxReaderBody" #>

public void visit(<#NodeName#> _<#NodeName#>)
{
    read_<#NodeName#>(_<#NodeName#>);
}

<# if SyntaxTreeNode 
<#ReadSyntaxTreeNode#>
#>

<# if NotSyntaxTreeNode
public void read_<#NodeName#>(<#NodeName#> _<#NodeName#>)
{
    read_<#BaseNodeName#>(_<#NodeName#>);
    
    <#ReadFields join "\n"#>
}#>