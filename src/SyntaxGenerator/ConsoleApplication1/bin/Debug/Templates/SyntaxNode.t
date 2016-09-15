<#set TemplateType = "SyntaxNode"#>

<#FieldNames = FieldName(kind: "Any", includeParents: true, syntaxOnly: false)#>
<#SelfFieldNames = FieldName(kind: "Any", includeParents: false, syntaxOnly: false)#>
<#SyntaxFieldNames = FieldName(kind: "Any", includeParents: true, syntaxOnly: true)#>
<#SimpleSyntaxFieldNames = FieldName(kind: "Simple", includeParents: true, syntaxOnly: true)#>
<#SimpleSelfFieldNames = FieldName(kind: "Simple", includeParents: false, syntaxOnly: false)#>
<#ListName = FieldName(kind: "List", includeParents: false, syntaxOnly: true)#>


<#FieldNames = FieldName(kind: "Any", includeParents: true, syntaxOnly: false)#>
<#FieldTypes = FieldType(kind: "Any", includeParents: true, syntaxOnly: false)#>
<#SelfFieldTypes = FieldType(kind: "Any", includeParents: false, syntaxOnly: false)#>
<#SelfFieldTypesWithSC = FieldType(kind: "Any", includeParents: false, syntaxOnly: false)#>
<#ListType = FieldType(kind: "List", includeParents: false, syntaxOnly: true)#>
<#ListElementType = ListElementType(includeParents: false, syntaxOnly: true)#>
<#SimpleSyntaxFieldTypes = FieldType(kind: "Simple", includeParents: true, syntaxOnly: true)#>
<#SimpleSelfFieldTypes = FieldType(kind: "Simple", includeParents: false, syntaxOnly: false)#>

<#SimpleConstructorParams = "{SelfFieldTypes} _{SelfFieldNames}" join ", "#>
<#SimpleConstructorBody = "this._{SelfFieldNames} = _{SelfFieldNames};" join "\n"#>


<#FullConstructorParams = "{FieldTypes} _{FieldNames}" join ", "#>
<#FullConstructorBody = "this._{FieldNames} = _{FieldNames};" join "\n"#>

<#SimpleFieldsDeclaration = "protected {SimpleSelfFieldTypes} _{SimpleSelfFieldNames};" join "\n"#>
<#ListFieldsDeclaration = "protected {ListType} _{ListName} = new {ListType}();" join "\n"#>

<#HasSimpleFields = HasFields(kind: "Simple", includeParents: true, syntaxOnly: true)#>
<#HasList = HasFields(kind: "List", includeParents: false, syntaxOnly: true)#>
<#HasSelfFields = HasFields(kind: "Any", includeParents: false, syntaxOnly: false)#>

<#PropertiesDeclaration = 
"public {SelfFieldTypes} {SelfFieldNames}
{{
    get {{ return _{SelfFieldNames}; }}
    set {{ _{SelfFieldNames} = value; }}
}}" join "\n\n"
#>

<#IndexerGetSwitchBody =
"case {IntSequence(from: 0)}:
    return {SimpleSyntaxFieldNames};" join "\n"
#>

<#IndexerGetListBody = 
"if({ListName} != null)
{{
    if(index_counter < {ListName}.Count)
    {{
        return {ListName}[index_counter];
    }}
}}" join "\n"
#>

<#IndexerSetSwitchBody =
"case {IntSequence(from: 0)}:
    {SimpleSyntaxFieldNames} = ({SimpleSyntaxFieldTypes})value;
    break;" join "\n"
#>

<#IndexerSetListBody = 
"if({ListName} != null)
{{
    if(index_counter < {ListName}.Count)
    {{
        {ListName}[index_counter] = ({ListElementType})value;
    }}
}}" join "\n"
#>

[Serializable]
public partial class <#NodeName#><#" : {BaseNodeName}"#>
{
    <#SimpleFieldsDeclaration#>
    
    <#ListFieldsDeclaration#>
    
    public <#NodeName#>() 
    {

    }
    
    <#if HasSelfFields
    public <#NodeName#>(<#SimpleConstructorParams#>)
    {
        <#SimpleConstructorBody#>
    }
        <#if NotSyntaxTreeNode
    public <#NodeName#>(<#SimpleConstructorParams#>, SourceContext sc)
    {
        <#SimpleConstructorBody#>
        _source_context = sc;
    }
        #>
    #>
    
    <# if HasInheritedFields
    public <#NodeName#>(<#FullConstructorParams#>)
    {
        <#FullConstructorBody#>
    }
    
    public <#NodeName#>(<#FullConstructorParams#>, SourceContext sc)
    {
        <#FullConstructorBody#>
        _source_context = sc;
    }#>
    
    <# if HasSingleList
    public <#NodeName#>(<#ListElementType#> elem, SourceContext sc = null)
    {
        Add(elem, sc);
    }
    #>
    
    <#PropertiesDeclaration#>
    
    ///<summary>
    ///Свойство для получения количества всех подузлов без элементов поля типа List
    ///</summary>
    public <#InheritanceModifier#> Int32 subnodes_without_list_elements_count
    {
        get { return <#SimpleFieldsCount#>; }
    }
    
    ///<summary>
    ///Свойство для получения количества всех подузлов. Подузлом также считается каждый элемент поля типа List
    ///</summary>
    public <#InheritanceModifier#> Int32 subnodes_count
    {
        get
        {
            return <#SimpleFieldsCount#><#" + ({ListName} == null ? 0 : {ListName}.Count)"#>;
        }
    }
    
    ///<summary>
    ///Индексатор для получения всех подузлов
    ///</summary>
    public <#InheritanceModifier#> syntax_tree_node this[Int32 ind]
    {
        get
        {
            if (subnodes_count == 0 || ind < 0 || ind > subnodes_count - 1)
                throw new IndexOutOfRangeException();
            <#if HasSimpleFields
            switch(ind)
            {
                <#IndexerGetSwitchBody#>
            }
            #>
            <#if HasList
            Int32 index_counter=ind - <#SimpleFieldsCount#>;
            <#IndexerGetListBody#>
            #>
            return null;
        }
        
        set
        {
            if(subnodes_count == 0 || ind < 0 || ind > subnodes_count - 1)
                throw new IndexOutOfRangeException();
            <#if HasSimpleFields
            switch(ind)
            {
                <#IndexerSetSwitchBody#>
            }
            #>
            <#if HasList
            Int32 index_counter=ind - <#SimpleFieldsCount#>;
            <#IndexerSetListBody#>
            #>
        }
    }
    
    ///<summary>
    ///Метод для обхода дерева посетителем
    ///</summary>
    ///<param name="visitor">Объект-посетитель.</param>
    ///<returns>Return value is void</returns>
    public <#InheritanceModifier#> void visit(IVisitor visitor)
    {
        visitor.visit(this);
    }
    
    <# if HasSingleList
    public void AddMany(params <#ListElementType#>[] els)
    {
        <#ListName#>.AddRange(els);
    }
    
    public bool Remove(<#ListElementType#> el)
    {
        return <#ListName#>.Remove(el);
    }
    
    private int FindIndexInList(<#ListElementType#> el)
    {
        var ind = <#ListName#>.FindIndex(x => x == el);
        if (ind == -1)
            throw new Exception(string.Format("У списка {0} не найден элемент {1} среди дочерних\n", this, el));
        return ind;
    }
    
    public void ReplaceInList(<#ListElementType#> el, <#ListElementType#> newel)
    {
        <#ListName#>[FindIndexInList(el)] = newel;
    }
    
    public void ReplaceInList(<#ListElementType#> el, IEnumerable<<#ListElementType#>> newels)
    {
        var ind = FindIndexInList(el);
        <#ListName#>.RemoveAt(ind);
        <#ListName#>.InsertRange(ind, newels);
    }
    
    public void InsertAfter(<#ListElementType#> el, <#ListElementType#> newel)
    {
        <#ListName#>.Insert(FindIndex(el) + 1, newel);
    }
    
    public void InsertBefore(<#ListElementType#> el, <#ListElementType#> newel)
    {
        <#ListName#>.Insert(FindIndex(el), newel);
    }
    
    public void InsertAfter(<#ListElementType#> el, IEnumerable<<#ListElementType#>> newels)
    {
        <#ListName#>.InsertRange(FindIndex(el) + 1, newels);
    }
    
    public void InsertBefore(<#ListElementType#> el, IEnumerable<<#ListElementType#>> newels)
    {
        <#ListName#>.InsertRange(FindIndex(el), newels);
    }
    
    public void AddFirst(<#ListElementType#> el)
    {
        <#ListName#>.Insert(0, el);
    }
    
    public void AddFirst(IEnumerable<<#ListElementType#>> els)
    {
        <#ListName#>.InsertRange(0, els);
    }
    
    public <#NodeName#> Add(<#ListElementType#> elem, SourceContext sc = null)
    {
        <#ListName#>.Add(elem);
        if (sc != null)
            source_context = sc;
        return this;
    }
    
    public int RemoveAll(Predicate<<#ListElementType#>> match)
    {
        return <#ListName#>.RemoveAll(match);
    }
    #>
}
