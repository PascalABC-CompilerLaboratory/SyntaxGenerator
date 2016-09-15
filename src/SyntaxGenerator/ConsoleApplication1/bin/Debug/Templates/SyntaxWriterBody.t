<# set TemplateType = "SyntaxWriterBody" #>

public void visit(<#NodeName#> _<#NodeName#>)
{
    bw.Write((Int16)<#NodeNumber#>);
    write_<#NodeName#>(_<#NodeName#>);
}

<# if SyntaxTreeNode 
<#WriteSyntaxTreeNode#>
#>

<# if NotSyntaxTreeNode
public void write_<#NodeName#>(<#NodeName#> _<#NodeName#>)
{
    write_<#BaseNodeName#>(_<#NodeName#>);
    
    <#WriteFields join "\n"#>
}#>