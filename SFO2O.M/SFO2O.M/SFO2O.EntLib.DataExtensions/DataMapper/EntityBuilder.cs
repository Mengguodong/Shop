using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;
using System.Linq.Expressions;
using System.CodeDom.Compiler;
using SFO2O.EntLib.DataExtensions.DataMapper.Schema;
using System.Reflection;
using SFO2O.EntLib.DataExtensions.Basic;

namespace SFO2O.EntLib.DataExtensions.DataMapper
{
    /// <summary>
    /// 实例对象生成器，依据<typeparamref name="T"/>生成<typeparamref name="T"/>类型的子类，子类在继承 <typeparamref name="T"/>实现覆盖它属性的同时还实现<see cref="IMapper"/>接口。
    /// </summary>
    internal static class EntityBuilder
    {
        /// <summary>
        /// 生成一个实体的继承类，重写实体中属性成员，并继承IEntity接口。
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="fields">实体中所有需要映射的成员字段</param>
        /// <param name="schema">架构信息</param>
        /// <returns></returns>
        public static Type BuilderEntityClass<TEntity>(IEnumerable<FieldMap<TEntity>> fields, BaseSchema schema)
        {
            CodeTypeDeclaration @class = BuilderIObjectMapEntityClass.CreateIEntity<TEntity>(fields, schema);
            // 生成代码
            return BuildType<TEntity>(@class);
        }

        /// <summary>
        /// 获取表达未成员类型信息
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="exp"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static PropertyInfo ExtractPropertyInfo<TEntity>(Expression<Func<TEntity, object>> exp, out string propertyName)
        {
            MemberExpression member = ExtractMemberExpression<TEntity>(exp);
            PropertyInfo propertyInfo = member.Member as PropertyInfo;
            if (propertyInfo == null)
                throw new MapperException("Lambda表达示不符合映射的标准，标准示例：x=>x.ID。");

            propertyName = member.Member.Name;
            return propertyInfo;
        }

        /// <summary>
        /// 获取表达未成员类型信息
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static MemberExpression ExtractMemberExpression<TEntity>(Expression<Func<TEntity, object>> exp)
        {
            MemberExpression member;
            if (exp.Body.NodeType == ExpressionType.Convert)
                member = (MemberExpression)((UnaryExpression)exp.Body).Operand;
            else if (exp.Body.NodeType == ExpressionType.MemberAccess)
                member = (MemberExpression)exp.Body;
            else
                throw new MapperException("Lambda表达示不符合映射的标准，标准示例：x=>x.ID。");
            //
            return member;
        }

        private static Type BuildType<TEntity>(CodeTypeDeclaration @class)
        {
            CodeNamespace nsp = CreateNamespace(typeof(TEntity).Namespace, @class);
            nsp.Imports.Add(new CodeNamespaceImport(typeof(TEntity).Namespace));
            nsp.Imports.Add(new CodeNamespaceImport(typeof(ItemValue).Namespace));

            CodeCompileUnit unit = new CodeCompileUnit();
            unit.Namespaces.Add(nsp);

            // 编译的选项参数
            CompilerParameters option = new CompilerParameters();
            option.GenerateExecutable = false;
#if DEBUG
            option.GenerateInMemory = false;
            option.IncludeDebugInformation = true;
#else
            option.GenerateInMemory = true;
            option.IncludeDebugInformation = false;
#endif
            option.ReferencedAssemblies.Add("System.dll");
            option.ReferencedAssemblies.Add("System.Core.dll");
            option.ReferencedAssemblies.Add(typeof(TEntity).Assembly.Location);
            option.ReferencedAssemblies.Add(typeof(EntityBuilder).Assembly.Location);
#if DEBUG
            string code = GenerateCode(unit);
#endif
            CompilerResults result = CodeDomProvider.CreateProvider("C#").CompileAssemblyFromDom(option, unit);
            foreach (CompilerError error in result.Errors)
            {
                if (!error.IsWarning)
                {
                    throw new MapperException(string.Format("代码生成失败。\n\t错误号：{1}。\n\t错误消息：\n\t{2}。\n\t源码：\n\t{3}", typeof(TEntity).FullName, error.ErrorNumber, error.ErrorText, GenerateCode(unit)));
                }
            }
            return result.CompiledAssembly.GetType(string.Concat(nsp.Name, ".", @class.Name));
        }

        private static CodeTypeDeclaration CreateClass(string name, params CodeTypeMember[] members)
        {
            CodeTypeDeclaration myclass = new CodeTypeDeclaration(name);
            myclass.Attributes = MemberAttributes.Public;
            myclass.Members.AddRange(members);
            return myclass;
        }

        private static CodeNamespace CreateNamespace(string name, params CodeTypeDeclaration[] types)
        {
            CodeNamespace nsp = new CodeNamespace(name);
            nsp.Imports.Add(new CodeNamespaceImport("System"));
            nsp.Imports.Add(new CodeNamespaceImport("System.Linq"));
            nsp.Types.AddRange(types);
            return nsp;
        }

        /// <summary>
        /// 生成C#语法代码(文本)
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        private static string GenerateCode(CodeCompileUnit unit)
        {
            StringBuilder sb = new StringBuilder();
            using (System.IO.StringWriter sw = new System.IO.StringWriter())
            {
                CodeDomProvider.CreateProvider("C#").GenerateCodeFromCompileUnit(unit, sw, null);
                return sw.ToString();
            }
        }

        private static class BuilderIObjectMapEntityClass
        {
            public static CodeTypeDeclaration CreateIEntity<T>(IEnumerable<FieldMap<T>> fields, BaseSchema schema)
            {
                var members = new List<CodeTypeMember>();
                members.AddRange(CreateIEntityMembers<T>());
                members.AddRange(CreateOverrideProperties<T>(fields));
                members.AddRange(CreateOtherMembers<T>(schema));

                string className = string.Concat(typeof(T).FullName.Replace('.', '_'), "_IEntity");
                CodeTypeDeclaration myclass = CreateClass(className, members.ToArray());
                myclass.BaseTypes.Add(new CodeTypeReference(typeof(T)));
                myclass.BaseTypes.Add(new CodeTypeReference(typeof(IEntity)));
                return myclass;
            }
            public static CodeTypeMember[] CreateOtherMembers<T>(BaseSchema schema)
            {
                List<CodeTypeMember> members = new List<CodeTypeMember>();
                //
                // 在实体类中声明：IndexInscriber
                CodeTypeReference type = new CodeTypeReference(typeof(IndexInscriber));
                CodeMemberField fieldIndexer = new CodeMemberField(type, "_Indexer");
                members.Add(fieldIndexer);
                // 声明IMapper
                CodeMemberField iMapperField = new CodeMemberField(typeof(IMapper<T>), "_IMapper");
                members.Add(iMapperField);
                // 声明importing(用于标识是否正在导入数据)
                CodeMemberField importingField = new CodeMemberField(typeof(bool), "_Importing");
                members.Add(importingField);
                //
                CodeConstructor defaultCtor = new CodeConstructor();
                defaultCtor.Attributes = MemberAttributes.Public;
                defaultCtor.Statements.Add(new CodeAssignStatement(
                    new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "_IMapper"),
                    new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(new CodeTypeReferenceExpression(typeof(Mapping)), "GetIMapper", new CodeTypeReference[] { new CodeTypeReference(typeof(T)) }))
                ));
                defaultCtor.Statements.Add(new CodeAssignStatement(
                    new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "_Indexer"),
                    new CodeObjectCreateExpression(typeof(IndexInscriber), new CodePrimitiveExpression(schema.FieldCount))
                ));
                members.Add(defaultCtor);

                //
                return members.ToArray();
            }
            public static CodeTypeMember[] CreateIEntityMembers<T>()
            {
                List<CodeTypeMember> members = new List<CodeTypeMember>();

                // 
                var thisRef = new CodeThisReferenceExpression();
                var propertySetValue = new CodePropertySetValueReferenceExpression();
                var fieldIMapper = new CodeFieldReferenceExpression(thisRef, "_IMapper");
                var fieldSchema = new CodeFieldReferenceExpression(fieldIMapper, "Schema");

                #region DataState OperationState{get;set;}

                CodeMemberField fieldOperationState = new CodeMemberField(new CodeTypeReference(typeof(DataState)), "_OperationState");
                CodeMemberProperty ptyOperationState = new CodeMemberProperty();
                ptyOperationState.Name = "OperationState";
                ptyOperationState.Type = new CodeTypeReference(typeof(DataState));
                ptyOperationState.PrivateImplementationType = new CodeTypeReference(typeof(IEntity));
                // code: return _OperationState;
                ptyOperationState.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(thisRef, "_OperationState")));
                // code: _OperationState = value;
                ptyOperationState.SetStatements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(thisRef, "_OperationState"), propertySetValue));
                members.Add(fieldOperationState);
                members.Add(ptyOperationState);

                #endregion

                #region Identity Identity{ get; }

                CodeMemberProperty ptyIdentity = new CodeMemberProperty();
                ptyIdentity.Name = "Identity";
                ptyIdentity.Type = new CodeTypeReference(typeof(Identity));
                ptyIdentity.PrivateImplementationType = new CodeTypeReference(typeof(IEntity));
                // code: return this._IMapper.Schema.Identity;
                ptyIdentity.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(fieldSchema, "Identity")));
                members.Add(ptyIdentity);

                #endregion

                #region string Name{ get{ return _IMapper.Schema.Name; } }

                CodeMemberProperty ptyName = new CodeMemberProperty();
                ptyName.Name = "Name";
                ptyName.Type = new CodeTypeReference(typeof(string));
                ptyName.PrivateImplementationType = new CodeTypeReference(typeof(IEntity));
                // code: return _IMapper.Schema.Name;
                ptyName.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(fieldSchema, "Name")));
                members.Add(ptyName);

                #endregion

                #region object this[string field] { get; set; }

                CodeMemberProperty ptyThisIndex = new CodeMemberProperty();
                ptyThisIndex.Name = "Item";
                ptyThisIndex.PrivateImplementationType = new CodeTypeReference(typeof(IEntity));
                ptyThisIndex.Type = new CodeTypeReference(typeof(object));
                ptyThisIndex.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(typeof(string)), "field"));
                // code:
                // IPropertyValue<T> ptyValue = this._IMapper[field];
                ptyThisIndex.GetStatements.Add(new CodeVariableDeclarationStatement(
                            typeof(IPropertyValue<T>),
                            "ptyValue",
                            new CodeArrayIndexerExpression(fieldIMapper, new CodeArgumentReferenceExpression("field"))));
                // code:
                // if(ptyValue==null){ return null; }
                ptyThisIndex.GetStatements.Add(new CodeConditionStatement(
                    new CodeBinaryOperatorExpression(new CodeVariableReferenceExpression("ptyValue"), CodeBinaryOperatorType.IdentityEquality, new CodePrimitiveExpression(null)),
                    new CodeMethodReturnStatement(new CodePrimitiveExpression(null))));
                // return ptyValue.GetValue(this);
                ptyThisIndex.GetStatements.Add(new CodeMethodReturnStatement(new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("ptyValue"), "GetValue", thisRef)));

                // 
                // IPropertyValue<T> ptyValue = this._IMapper[field];
                ptyThisIndex.SetStatements.Add(new CodeVariableDeclarationStatement(
                            typeof(IPropertyValue<T>),
                            "ptyValue",
                            new CodeArrayIndexerExpression(fieldIMapper, new CodeArgumentReferenceExpression("field"))));
                // code:
                // if(ptyValue!=null)
                // {
                //   ptyValue.SetValue(this,value);
                // }
                ptyThisIndex.SetStatements.Add(new CodeConditionStatement(
                    new CodeBinaryOperatorExpression(new CodeVariableReferenceExpression("ptyValue"), CodeBinaryOperatorType.IdentityInequality, new CodePrimitiveExpression(null)),
                    new CodeExpressionStatement(new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("ptyValue"), "SetValue", thisRef, propertySetValue)))
                );

                //
                members.Add(ptyThisIndex);

                #endregion

                #region string[] KeyFields { get; }

                CodeMemberProperty ptyKeyFields = new CodeMemberProperty();
                ptyKeyFields.Name = "KeyFields";
                ptyKeyFields.Type = new CodeTypeReference(typeof(string[]));
                ptyKeyFields.PrivateImplementationType = new CodeTypeReference(typeof(IEntity));
                // code: return this._IMapper.Schema.GetKeyFields();
                ptyKeyFields.GetStatements.Add(new CodeMethodReturnStatement(new CodeMethodInvokeExpression(fieldSchema, "GetKeyFields")));

                //
                members.Add(ptyKeyFields);

                #endregion

                #region string[] Fields { get; }

                CodeMemberProperty ptyFields = new CodeMemberProperty();
                ptyFields.Name = "Fields";
                ptyFields.Type = new CodeTypeReference(typeof(string[]));
                ptyFields.PrivateImplementationType = new CodeTypeReference(typeof(IEntity));
                // code: return this._IMapper.Schema.GetAllFields();
                ptyFields.GetStatements.Add(new CodeMethodReturnStatement(new CodeMethodInvokeExpression(fieldSchema, "GetAllFields")));

                //
                members.Add(ptyFields);

                #endregion

                #region bool ValueModified { get; }

                CodeMemberProperty ptyValueModified = new CodeMemberProperty();
                ptyValueModified.Name = "ValueModified";
                ptyValueModified.Type = new CodeTypeReference(typeof(bool));
                ptyValueModified.PrivateImplementationType = new CodeTypeReference(typeof(IEntity));
                // code: return this._index.HasValue();
                ptyValueModified.GetStatements.Add(new CodeMethodReturnStatement(new CodeMethodInvokeExpression(new CodeFieldReferenceExpression(thisRef, "_Indexer"), "HasValue")));

                //
                members.Add(ptyValueModified);

                #endregion

                #region IEnumerable<string> GetModifiedFields();

                CodeMemberMethod methodGetModifiedFields = new CodeMemberMethod();
                methodGetModifiedFields.Name = "GetModifiedFields";
                methodGetModifiedFields.PrivateImplementationType = new CodeTypeReference(typeof(IEntity));
                methodGetModifiedFields.ReturnType = new CodeTypeReference(typeof(IEnumerable<string>));
                // code: short[] index = this._indexer.ValueCount();
                methodGetModifiedFields.Statements.Add(new CodeVariableDeclarationStatement(typeof(short[]), "index", new CodeMethodInvokeExpression(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "_Indexer"), "ValueIndex")));
                // code: IList<string> list = new List<string>(index.Length);
                methodGetModifiedFields.Statements.Add(new CodeVariableDeclarationStatement(typeof(IList<string>), "list", new CodeObjectCreateExpression(new CodeTypeReference(typeof(List<string>)), new CodeFieldReferenceExpression(new CodeVariableReferenceExpression("index"), "Length"))));
                // code: for(int i=0;i<index.Length;i=i+1) { list.Add(_IMapper.Schema.FieldName(index[i])); }
                methodGetModifiedFields.Statements.Add(new CodeIterationStatement(
                    new CodeVariableDeclarationStatement(new CodeTypeReference(typeof(int)), "i", new CodePrimitiveExpression(0)),
                    new CodeBinaryOperatorExpression(new CodeVariableReferenceExpression("i"), CodeBinaryOperatorType.LessThan, new CodeFieldReferenceExpression(new CodeVariableReferenceExpression("index"), "Length")),
                    new CodeAssignStatement(new CodeVariableReferenceExpression("i"), new CodeBinaryOperatorExpression(new CodeVariableReferenceExpression("i"), CodeBinaryOperatorType.Add, new CodePrimitiveExpression(1))),
                    new CodeStatement[] 
                    { 
                        new CodeExpressionStatement(new CodeMethodInvokeExpression(
                            new CodeVariableReferenceExpression("list"),
                            "Add",
                            new CodeMethodInvokeExpression(fieldSchema,"FieldName",new CodeArrayIndexerExpression(new CodeVariableReferenceExpression("index"),new CodeVariableReferenceExpression("i")))
                        ))
                    }
                ));
                // code: return list;
                methodGetModifiedFields.Statements.Add(new CodeMethodReturnStatement(new CodeVariableReferenceExpression("list")));

                //
                members.Add(methodGetModifiedFields);

                #endregion

                #region bool IsPrimaryKey(string field);

                CodeMemberMethod ptyIsPrimaryKey = new CodeMemberMethod();
                ptyIsPrimaryKey.Name = "IsPrimaryKey";
                ptyIsPrimaryKey.PrivateImplementationType = new CodeTypeReference(typeof(IEntity));
                ptyIsPrimaryKey.ReturnType = new CodeTypeReference(typeof(bool));
                ptyIsPrimaryKey.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(typeof(string)), "field"));
                ptyIsPrimaryKey.Statements.Add(new CodeMethodReturnStatement(new CodeMethodInvokeExpression(fieldSchema, "IsKey", new CodeArgumentReferenceExpression("field"))));

                //
                members.Add(ptyIsPrimaryKey);

                #endregion

                #region IEnumerable<ItemValue> GetValues(IEnumerable<string> fields);

                CodeMemberMethod methodGetValues = new CodeMemberMethod();
                methodGetValues.Name = "GetValues";
                methodGetValues.PrivateImplementationType = new CodeTypeReference(typeof(IEntity));
                methodGetValues.ReturnType = new CodeTypeReference(typeof(IEnumerable<ItemValue>));
                methodGetValues.Parameters.Add(new CodeParameterDeclarationExpression(typeof(IEnumerable<string>), "fields"));
                // code: string[] fd = fields.ToArray();
                methodGetValues.Statements.Add(new CodeVariableDeclarationStatement(typeof(string[]), "fd",
                    new CodeMethodInvokeExpression(new CodeArgumentReferenceExpression("fields"), "ToArray")));
                // code: List<ItemValue> list = new List<ItemValue>(ds.Length);
                methodGetValues.Statements.Add(new CodeVariableDeclarationStatement(typeof(List<ItemValue>), "list",
                    new CodeObjectCreateExpression(typeof(List<ItemValue>), new CodeFieldReferenceExpression(new CodeVariableReferenceExpression("fd"), "Length"))));
                // code: 
                // for(int i=0;i<fd.Length;i++) { 
                //  IPropertyValue<T> ptyValue = _IMapper[fd[i]];
                //  if(ptyValue != null)
                //      list.Add(new ItemValue(fd[i],ptyValue.GetValue(this))); 
                // }
                methodGetValues.Statements.Add(new CodeIterationStatement(
                    new CodeVariableDeclarationStatement(new CodeTypeReference(typeof(int)), "i", new CodePrimitiveExpression(0)),
                    new CodeBinaryOperatorExpression(new CodeVariableReferenceExpression("i"), CodeBinaryOperatorType.LessThan, new CodeFieldReferenceExpression(new CodeVariableReferenceExpression("fd"), "Length")),
                    new CodeAssignStatement(new CodeVariableReferenceExpression("i"), new CodeBinaryOperatorExpression(new CodeVariableReferenceExpression("i"), CodeBinaryOperatorType.Add, new CodePrimitiveExpression(1))),
                    new CodeStatement[] 
                    { 
                        new CodeVariableDeclarationStatement(
                            typeof(IPropertyValue<T>),
                            "ptyValue",
                            new CodeArrayIndexerExpression(fieldIMapper,new CodeArrayIndexerExpression(new CodeVariableReferenceExpression("fd"),new CodeVariableReferenceExpression("i")))),
                        new CodeConditionStatement(
                            new CodeBinaryOperatorExpression(new CodeVariableReferenceExpression("ptyValue"),CodeBinaryOperatorType.IdentityInequality,new CodePrimitiveExpression(null)),
                            new CodeExpressionStatement(new CodeMethodInvokeExpression(
                                new CodeVariableReferenceExpression("list"),
                                "Add",
                                new CodeObjectCreateExpression(
                                    typeof(ItemValue),
                                    new CodeArrayIndexerExpression(new CodeVariableReferenceExpression("fd"),new CodeVariableReferenceExpression("i")),
                                    new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("ptyValue"),"GetValue",thisRef)
                                )
                            ))
                        )
                    }
                ));
                // code: return list;
                methodGetValues.Statements.Add(new CodeMethodReturnStatement(new CodeVariableReferenceExpression("list")));
                //
                members.Add(methodGetValues);

                #endregion

                #region void Importing();

                CodeMemberMethod methodImporting = new CodeMemberMethod();
                methodImporting.Name = "Importing";
                methodImporting.PrivateImplementationType = new CodeTypeReference(typeof(IEntity));
                methodImporting.Statements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(thisRef, "_Importing"), new CodePrimitiveExpression(true)));
                methodImporting.Statements.Add(new CodeMethodInvokeExpression(
                    new CodeMethodReferenceExpression(
                        new CodeFieldReferenceExpression(
                            new CodeThisReferenceExpression(), "_Indexer"), "Clean")));
                //
                members.Add(methodImporting);

                #endregion

                #region void Imported();

                CodeMemberMethod methodImported = new CodeMemberMethod();
                methodImported.Name = "Imported";
                methodImported.PrivateImplementationType = new CodeTypeReference(typeof(IEntity));
                methodImported.Statements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(thisRef, "_Importing"), new CodePrimitiveExpression(false)));
                //
                members.Add(methodImported);

                #endregion

                //
                return members.ToArray();
            }
            public static CodeTypeMember[] CreateOverrideProperties<T>(IEnumerable<FieldMap<T>> fields)
            {
                IList<CodeTypeMember> members = new List<CodeTypeMember>(fields.Count());

                // 重写基类（实体对象）的属性。
                foreach (var field in fields)
                {
                    var exp = field.Expression;
                    string propertyName;
                    PropertyInfo propertyInfo = ExtractPropertyInfo<T>(exp, out propertyName);
                    Check.Valid(!propertyInfo.GetSetMethod().IsVirtual, "类型[{0}]中的属性[{1}]必须标记为[virtual]。", propertyInfo.PropertyType.FullName, propertyName);

                    //
                    CodeMemberProperty property = new CodeMemberProperty();
                    property.Name = propertyName;
                    property.Type = new CodeTypeReference(propertyInfo.PropertyType);
                    property.Attributes = MemberAttributes.Override | MemberAttributes.Public;

                    // 向属性的get方法增加代码
                    // code: return base.PropertyName;
                    property.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeBaseReferenceExpression(), propertyName)));
                    // 向属性的set方法增加代码
                    // code:
                    // if (_Importing){
                    //      base.PropertyName == value;
                    //      return;
                    // }
                    // if (base.PropertyName == value){
                    //      if(value == default(type))
                    //          this._index.SetValue(index of field);
                    //      return;
                    // }
                    // this._index.SetValue(index of field);
                    // base.PropertyName = value;
                    property.SetStatements.Add(
                        new CodeConditionStatement(
                            new CodeBinaryOperatorExpression(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "_Importing"), CodeBinaryOperatorType.IdentityEquality, new CodePrimitiveExpression(true)),
                            new CodeAssignStatement(
                                new CodeFieldReferenceExpression(new CodeBaseReferenceExpression(), propertyName),
                                new CodePropertySetValueReferenceExpression()
                            ),
                            new CodeMethodReturnStatement()
                        )
                    );
                    var methodIndexerCall = new CodeMethodInvokeExpression(
                        new CodeMethodReferenceExpression(
                            new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "_Indexer"),
                            "SetValue"),
                            new CodePrimitiveExpression(field.Index));
                    var condtion1 = new CodeConditionStatement(
                        new CodeBinaryOperatorExpression(
                            new CodePropertySetValueReferenceExpression(),
                            CodeBinaryOperatorType.IdentityEquality,
                            new CodeDefaultValueExpression(property.Type))
                        );
                    condtion1.TrueStatements.Add(methodIndexerCall);

                    property.SetStatements.Add(
                        new CodeConditionStatement(
                            new CodeBinaryOperatorExpression(new CodeFieldReferenceExpression(new CodeBaseReferenceExpression(), propertyName), CodeBinaryOperatorType.ValueEquality, new CodePropertySetValueReferenceExpression()),
                            condtion1,
                            new CodeMethodReturnStatement()
                        )
                    );
                    property.SetStatements.Add(methodIndexerCall);
                    property.SetStatements.Add(new CodeAssignStatement(
                        new CodeFieldReferenceExpression(new CodeBaseReferenceExpression(), propertyName),
                        new CodePropertySetValueReferenceExpression()));

                    //
                    members.Add(property);
                }
                return members.ToArray();
            }
        }
    }
}
