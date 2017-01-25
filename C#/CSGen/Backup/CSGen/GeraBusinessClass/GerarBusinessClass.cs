﻿namespace CSGen.GeraBusinessClass
{
    using CSGen;
    using CSGen.Code;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Text;

    internal class GerarBusinessClass
    {
        private static List<AtributesForClass> camposClass = new List<AtributesForClass>();
        private static string parametros = "";
        private static string parametrosLimp = "";
        private static StringCollection sc = new StringCollection();

        private static void BildCamposClass(SqlTable sqlTable)
        {
            int num = 0;
            foreach (SqlColumn column in sqlTable.Colunas)
            {
                AtributesForClass class2;
                if ((column.IsPk && !column.IsFk) && !column.IsUk)
                {
                    char ch = column.Name[0];
                    class2 = new AtributesForClass(column.NetDataType, ch.ToString().ToLower() + column.Name.Substring(1), false, "", "PK", column.IsIdentity, null);
                    if (!ExisteCampo(class2))
                    {
                        camposClass.Add(class2);
                    }
                    foreach (SqlReference reference in column.GetReferencesForPk(sqlTable))
                    {
                        if (reference.Ignore)
                        {
                            continue;
                        }
                        if (reference.IsCollection)
                        {
                            SqlTable sqlTableNo = SqlTable.GetTable(reference.FkTable);
                            SqlTable tableToNo = GetTableToNo(sqlTable, sqlTableNo);
                            if (!sqlTableNo.IsTableNo)
                            {
                                char ch2 = reference.ClassBusinessName[0];
                                class2 = new AtributesForClass(reference.ClassBusinessName, ch2.ToString().ToLower() + reference.ClassBusinessName.Substring(1), true, "L", "PK", false, null);
                                if (!ExisteCampo(class2))
                                {
                                    camposClass.Add(class2);
                                }
                            }
                            else
                            {
                                char ch3 = tableToNo.ClassBusinessNome[0];
                                class2 = new AtributesForClass(tableToNo.ClassBusinessNome, ch3.ToString().ToLower() + tableToNo.ClassBusinessNome.Substring(1), true, "L", "PK", false, null);
                                if (!ExisteCampo(class2))
                                {
                                    camposClass.Add(class2);
                                }
                            }
                            continue;
                        }
                        char ch4 = reference.ClassBusinessName[0];
                        class2 = new AtributesForClass(reference.ClassBusinessName, ch4.ToString().ToLower() + reference.ClassBusinessName.Substring(1), false, "R", "PK", false, null);
                        if (!ExisteCampo(class2))
                        {
                            camposClass.Add(class2);
                        }
                    }
                    continue;
                }
                if ((!column.IsPk && column.IsFk) && !column.IsUk)
                {
                    foreach (SqlReference reference2 in column.GetReferencesForFk(sqlTable))
                    {
                        if (reference2.Ignore)
                        {
                            continue;
                        }
                        if (reference2.IsCollection && (reference2.FkTable != reference2.PkTable))
                        {
                            char ch5 = reference2.ClassBusinessName[0];
                            class2 = new AtributesForClass(reference2.ClassBusinessName, ch5.ToString().ToUpper() + reference2.ClassBusinessName.Substring(1), true, "L", "FK", false, null);
                            if (!ExisteCampo(class2))
                            {
                                camposClass.Add(class2);
                            }
                            continue;
                        }
                        if (num == 0)
                        {
                            char ch6 = reference2.ClassBusinessName[0];
                            class2 = new AtributesForClass(reference2.ClassBusinessName, ch6.ToString().ToUpper() + reference2.ClassBusinessName.Substring(1), false, "R", "FK", false, sqlTable.Colunas[0].Name);
                        }
                        else
                        {
                            char ch7 = reference2.ClassBusinessName[0];
                            class2 = new AtributesForClass(reference2.ClassBusinessName, ch7.ToString().ToUpper() + reference2.ClassBusinessName.Substring(1), false, "R", "FK", false, sqlTable.Colunas[1].Name);
                        }
                        if (!ExisteCampo(class2))
                        {
                            camposClass.Add(class2);
                        }
                        if (sqlTable.IsTableNo)
                        {
                            if (num == 0)
                            {
                                char ch8 = reference2.ClassBusinessName[0];
                                class2 = new AtributesForClass(reference2.ClassBusinessName, ch8.ToString().ToUpper() + reference2.ClassBusinessName.Substring(1), true, "L", "FK", false, sqlTable.Colunas[1].Name);
                            }
                            else
                            {
                                char ch9 = reference2.ClassBusinessName[0];
                                class2 = new AtributesForClass(reference2.ClassBusinessName, ch9.ToString().ToUpper() + reference2.ClassBusinessName.Substring(1), true, "L", "FK", false, sqlTable.Colunas[0].Name);
                            }
                            num++;
                        }
                        if (!ExisteCampo(class2))
                        {
                            camposClass.Add(class2);
                        }
                    }
                    continue;
                }
                if ((!column.IsPk && !column.IsFk) && column.IsUk)
                {
                    char ch10 = column.Name[0];
                    class2 = new AtributesForClass(column.NetDataType, ch10.ToString().ToLower() + column.Name.Substring(1), false, "", "UK", column.IsIdentity, null);
                    if (!ExisteCampo(class2))
                    {
                        camposClass.Add(class2);
                    }
                    foreach (SqlReference reference3 in column.GetReferencesForPk(sqlTable))
                    {
                        if (reference3.Ignore)
                        {
                            continue;
                        }
                        if (reference3.IsCollection)
                        {
                            char ch11 = reference3.ClassBusinessName[0];
                            class2 = new AtributesForClass(reference3.ClassBusinessName, ch11.ToString().ToLower() + reference3.ClassBusinessName.Substring(1), true, "L", "UK", false, null);
                            if (!ExisteCampo(class2))
                            {
                                camposClass.Add(class2);
                            }
                            continue;
                        }
                        char ch12 = reference3.ClassBusinessName[0];
                        class2 = new AtributesForClass(reference3.ClassBusinessName, ch12.ToString().ToLower() + reference3.ClassBusinessName.Substring(1), false, "R", "UK", false, null);
                        if (!ExisteCampo(class2))
                        {
                            camposClass.Add(class2);
                        }
                    }
                    continue;
                }
                char ch13 = column.Name[0];
                class2 = new AtributesForClass(column.NetDataType, ch13.ToString().ToLower() + column.Name.Substring(1), false, "", "", column.IsIdentity, null);
                if (!ExisteCampo(class2))
                {
                    camposClass.Add(class2);
                }
            }
        }

        private static bool ExisteCampo(AtributesForClass campo)
        {
            foreach (AtributesForClass class2 in camposClass)
            {
                if ((class2.ClearName.ToLower() == campo.ClearName.ToLower()) && (class2.IniName == campo.IniName))
                {
                    return true;
                }
            }
            return false;
        }

        private static string GerarAtributos()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(Tab(2) + "#region Atributos");
            builder.AppendLine(Tab(2) + "private DbTransaction _transaction;");
            builder.AppendLine(Tab(2) + "private bool _isTransaction;");
            builder.AppendLine(Tab(2) + "private bool _isFull;");
            foreach (AtributesForClass class2 in camposClass)
            {
                char ch = class2.ClearName[0];
                string str = ch.ToString().ToUpper() + class2.ClearName.Substring(1);
                if (class2.IniList != null)
                {
                    builder.AppendLine(Tab(2) + "private " + ((class2.IniName == "L") ? "L" : "") + class2.Tipo + " _" + class2.IniName + str + ";");
                }
                else
                {
                    builder.AppendLine(Tab(2) + "private " + class2.Tipo + " _" + class2.IniName + class2.ClearName + ";");
                }
            }
            builder.AppendLine(Tab(2) + "#endregion");
            return builder.ToString();
        }

        public static string GerarClasse(SqlTable sqlTable)
        {
            camposClass.Clear();
            BildCamposClass(sqlTable);
            GerarParametros();
            string str = GerarCorpo(sqlTable);
            string newValue = GerarConstrutores(sqlTable);
            string str3 = GerarAtributos();
            string str4 = GerarPropriedades(sqlTable);
            string str5 = GerarMembrosEstaticos(sqlTable);
            string str6 = GerarMetodos(sqlTable);
            return str.Replace("*construtor*", newValue).Replace("*atributos*", str3).Replace("*propriedades*", str4).Replace("*membrosestaticos*", str5).Replace("*metodos*", str6);
        }

        private static string GerarConstrutores(SqlTable sqlTable)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(Tab(2) + "#region Construtores");
            builder.AppendLine(Tab(2) + string.Format("/// <summary>", new object[0]));
            builder.AppendLine(Tab(2) + string.Format("/// Contrutor padr\x00e3o sem argumentos", new object[0]));
            builder.AppendLine(Tab(2) + string.Format("/// </summary>", new object[0]));
            builder.AppendLine(Tab(2) + string.Format("/// <remarks>Danilo Aparecido</remarks>", new object[0]));
            builder.AppendLine(Tab(2) + string.Format("public {0}()", sqlTable.ClassBusinessNome) + " { }");
            if (!sqlTable.IsTableNo)
            {
                builder.AppendLine(Tab(2) + string.Format("/// <summary>", new object[0]));
                builder.AppendLine(Tab(2) + string.Format("/// Contrutor padr\x00e3o com argumento identity e preenchido", new object[0]));
                builder.AppendLine(Tab(2) + string.Format("/// </summary>", new object[0]));
                builder.AppendLine(Tab(2) + string.Format("/// <param name=\"" + sqlTable.GetIdentityColumn().Name + "_\"> Seta o atributo _" + sqlTable.GetIdentityColumn().Name + " do objeto " + sqlTable.ClassBusinessNome + "</param>", new object[0]));
                builder.AppendLine(Tab(2) + string.Format("/// <param name=\"isFull_\"> Seta o atributo _isFull do objeto " + sqlTable.ClassBusinessNome + ", false: preenche o objeto com os dados da base, true: Utiliza o objeto atual</param>", new object[0]));
                builder.AppendLine(Tab(2) + string.Format("/// <remarks>Danilo Aparecido</remarks>", new object[0]));
                builder.AppendLine(Tab(2) + string.Format("public {0}(" + sqlTable.GetIdentityColumn().SqlDataType + "? " + sqlTable.GetIdentityColumn().Name + "_, bool isFull_)", sqlTable.ClassBusinessNome) + " \n\t\t{\n\t\t\tthis._" + sqlTable.GetIdentityColumn().Name + " = " + sqlTable.GetIdentityColumn().Name + "_;\n\t\t\tthis._isFull = isFull_;\n\t\t}");
            }
            builder.AppendLine(Tab(2) + string.Format("/// <summary>", new object[0]));
            builder.AppendLine(Tab(2) + string.Format("/// Contrutor padr\x00e3o com argumentos", new object[0]));
            builder.AppendLine(Tab(2) + string.Format("/// </summary>", new object[0]));
            string[] strArray = parametrosLimp.Split(new char[] { ',' });
            for (int i = 0; i < strArray.Length; i++)
            {
                builder.AppendLine(Tab(2) + string.Format("/// <param name=\"" + strArray[i] + "\"> Seta o atributo _" + strArray[i] + " do objeto " + sqlTable.ClassBusinessNome + "</param>", new object[0]));
            }
            builder.AppendLine(Tab(2) + string.Format("/// <remarks>Danilo Aparecido</remarks>", new object[0]));
            builder.AppendLine(Tab(2) + string.Format("public {0}({1})", sqlTable.ClassBusinessNome, parametros));
            builder.AppendLine(Tab(2) + "{");
            foreach (AtributesForClass class2 in camposClass)
            {
                if ((class2.IniName == "") || (class2.IniName == "R"))
                {
                    builder.AppendLine(Tab(3) + "this._" + class2.IniName + class2.ClearName + " = " + class2.IniName + class2.ClearName + "_;");
                }
            }
            builder.AppendLine(Tab(3) + "this._isFull = isFull_;");
            builder.AppendLine(Tab(2) + "}");
            builder.AppendLine(Tab(2) + "#endregion");
            return builder.ToString();
        }

        private static string GerarCorpo(SqlTable sqlTable)
        {
            string classDataBaseNome = sqlTable.ClassDataBaseNome;
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("using System;");
            builder.AppendLine("using System.Collections.Generic;");
            builder.AppendLine("using System.Data.Common;");
            builder.AppendLine();
            builder.AppendLine(string.Format("namespace {0}Business", Program.prefixNamespace));
            builder.AppendLine("{");
            builder.AppendLine(Tab(1) + "#region Class List " + sqlTable.ClassBusinessNome);
            builder.AppendLine(Tab(1) + "/// <summary>");
            builder.AppendLine(Tab(1) + "/// Classe para armazenar uma cole\x00e7\x00e3o de " + sqlTable.ClassBusinessNome.ToLower() + "");
            builder.AppendLine(Tab(1) + "/// </summary>");
            builder.AppendLine(Tab(1) + "/// <remarks>Danilo Aparecido</remarks>");
            builder.AppendLine(Tab(1) + "public class L" + sqlTable.ClassBusinessNome + " : List<" + sqlTable.ClassBusinessNome + "> {}");
            builder.AppendLine(Tab(1) + "#endregion");
            builder.AppendLine();
            builder.AppendLine(Tab(1) + "#region Class " + sqlTable.ClassBusinessNome);
            builder.AppendLine(Tab(1) + "/// <summary>");
            builder.AppendLine(Tab(1) + "///  Classe de opera\x00e7\x00f5es do " + sqlTable.ClassBusinessNome.ToLower() + "");
            builder.AppendLine(Tab(1) + "/// </summary>");
            builder.AppendLine(Tab(1) + "/// <remarks>Danilo Aparecido</remarks>");
            builder.AppendLine(Tab(1) + "public class " + sqlTable.ClassBusinessNome);
            builder.AppendLine(Tab(1) + "{");
            builder.AppendLine("*construtor*");
            builder.AppendLine("*atributos*");
            builder.AppendLine("*propriedades*");
            builder.AppendLine("*membrosestaticos*");
            builder.AppendLine("*metodos*");
            builder.AppendLine(Tab(1) + "}");
            builder.AppendLine(Tab(1) + "#endregion");
            builder.AppendLine("}");
            return builder.ToString();
        }

        private static string GerarMembrosEstaticos(SqlTable sqlTable)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(Tab(2) + "#region Metodos Estaticos");
            builder.AppendLine(Tab(2) + "/// <summary>");
            builder.AppendLine(Tab(2) + "/// Metodo interno para setar e retorna um objeto pedido atrav\x00e9s de um DataReader");
            builder.AppendLine(Tab(2) + "/// </summary>");
            builder.AppendLine(Tab(2) + "/// <param name=\"dr\">DataReader de " + sqlTable.ClassBusinessNome.ToLower() + "</param>");
            builder.AppendLine(Tab(2) + "/// <param name=\"obj" + sqlTable.ClassBusinessNome + "\">Refer\x00eancia para objeto " + sqlTable.ClassBusinessNome + "</param>");
            builder.AppendLine(Tab(2) + "protected internal static " + sqlTable.ClassDataBaseNome + " setObject(DbDataReader dr, " + sqlTable.ClassBusinessNome + " obj" + sqlTable.ClassBusinessNome + ")");
            builder.AppendLine(Tab(2) + "{");
            builder.AppendLine(Tab(3) + "try");
            builder.AppendLine(Tab(3) + "{");
            foreach (SqlColumn column in sqlTable.Colunas)
            {
                string[] strArray3;
                char ch3;
                if (column.NetDataType == "string")
                {
                    if (column.Null)
                    {
                        builder.AppendLine(Tab(4) + "obj" + sqlTable.ClassDataBaseNome + "._" + column.Name.ToLower() + " = (dr[\"" + column.Name + "\"].ToString() == \"\" ? null : dr[\"" + column.Name + "\"].ToString());");
                    }
                    else
                    {
                        builder.AppendLine(Tab(4) + "obj" + sqlTable.ClassDataBaseNome + "._" + column.Name.ToLower() + " = dr[\"" + column.Name + "\"].ToString();");
                    }
                    continue;
                }
                if (column.Null)
                {
                    string str = getReference(sqlTable.References, column.Name);
                    if (str != "")
                    {
                        strArray3 = new string[15];
                        strArray3[0] = Tab(4);
                        strArray3[1] = "obj";
                        strArray3[2] = sqlTable.ClassDataBaseNome;
                        strArray3[3] = ".R";
                        strArray3[4] = str;
                        strArray3[5] = ".";
                        ch3 = column.Name[0];
                        strArray3[6] = ch3.ToString().ToUpper();
                        strArray3[7] = column.Name.Substring(1);
                        strArray3[8] = " = (dr[\"";
                        strArray3[9] = column.Name;
                        strArray3[10] = "\"].ToString() == \"\" ? null : (";
                        strArray3[11] = column.NetDataType;
                        strArray3[12] = ")dr[\"";
                        strArray3[13] = column.Name;
                        strArray3[14] = "\"]);";
                        builder.AppendLine(string.Concat(strArray3));
                    }
                    else
                    {
                        builder.AppendLine(Tab(4) + "obj" + sqlTable.ClassDataBaseNome + "._" + column.Name + " = (dr[\"" + column.Name + "\"].ToString() == \"\" ? null : (" + column.NetDataType + ")dr[\"" + column.Name + "\"]);");
                    }
                    continue;
                }
                if (sqlTable.IsTableNo)
                {
                    strArray3 = new string[13];
                    strArray3[0] = Tab(4);
                    strArray3[1] = "obj";
                    strArray3[2] = sqlTable.ClassDataBaseNome;
                    strArray3[3] = ".R";
                    strArray3[4] = getReference(sqlTable.References, column.Name);
                    strArray3[5] = ".";
                    ch3 = column.Name[0];
                    strArray3[6] = ch3.ToString().ToUpper();
                    strArray3[7] = column.Name.Substring(1);
                    strArray3[8] = " = (";
                    strArray3[9] = column.NetDataType;
                    strArray3[10] = ")dr[\"";
                    strArray3[11] = column.Name;
                    strArray3[12] = "\"];";
                    builder.AppendLine(string.Concat(strArray3));
                }
                else
                {
                    builder.AppendLine(Tab(4) + "obj" + sqlTable.ClassDataBaseNome + "._" + column.Name + " = (" + column.NetDataType + ")dr[\"" + column.Name + "\"];");
                }
            }
            builder.AppendLine(Tab(4) + "obj" + sqlTable.ClassDataBaseNome + "._isFull = true;");
            builder.AppendLine(Tab(4) + "return obj" + sqlTable.ClassDataBaseNome + ";");
            builder.AppendLine(Tab(3) + "}");
            builder.AppendLine(Tab(3) + "catch{ return null; }");
            builder.AppendLine(Tab(2) + "}");
            builder.AppendLine();
            builder.AppendLine(Tab(2) + "/// <summary>");
            builder.AppendLine(Tab(2) + "/// Metodo interno para setar a transa\x00e7\x00e3o para o objeto");
            builder.AppendLine(Tab(2) + "/// Obs: utilizar somente em metodos de insert, delete, update");
            builder.AppendLine(Tab(2) + "/// </summary>");
            builder.AppendLine(Tab(2) + "/// <param name=\"obj" + sqlTable.ClassBusinessNome + "\">Refer\x00eancia para o objeto " + sqlTable.ClassBusinessNome + "</param>");
            builder.AppendLine(Tab(2) + "/// <param name=\"db\">Refer\x00eancia para DataBase " + sqlTable.ClassDataBaseNome + "</param>");
            builder.AppendLine(Tab(2) + "protected internal static void setTransaction(" + sqlTable.ClassBusinessNome + " obj" + sqlTable.ClassBusinessNome + ", DataBase." + sqlTable.ClassBusinessNome + " db)");
            builder.AppendLine(Tab(2) + "{");
            builder.AppendLine(Tab(3) + "if (obj" + sqlTable.ClassBusinessNome + ".Transaction != null)");
            builder.AppendLine(Tab(3) + "{");
            builder.AppendLine(Tab(4) + "db.sqlTrans = obj" + sqlTable.ClassBusinessNome + ".Transaction;");
            builder.AppendLine(Tab(4) + "obj" + sqlTable.ClassBusinessNome + ".IsTransaction = true;");
            builder.AppendLine(Tab(3) + "}");
            builder.AppendLine(Tab(3) + "db.isTransaction = obj" + sqlTable.ClassBusinessNome + ".IsTransaction;");
            builder.AppendLine(Tab(2) + "}");
            builder.AppendLine();
            if (!sqlTable.IsTableNo)
            {
                builder.AppendLine(Tab(2) + "/// <summary>");
                builder.AppendLine(Tab(2) + "/// Metodo interno para setar e retornar um " + sqlTable.ClassBusinessNome.ToLower() + " atrav\x00e9s de uma refer\x00eancia de " + sqlTable.ClassBusinessNome.ToLower() + "");
                builder.AppendLine(Tab(2) + "/// </summary>");
                builder.AppendLine(Tab(2) + "/// <param name=\"obj" + sqlTable.ClassBusinessNome + "\">Refer\x00eancia para o objeto " + sqlTable.ClassBusinessNome + "</param>");
                builder.AppendLine(Tab(2) + "protected internal static " + sqlTable.ClassBusinessNome + " getByObject(" + sqlTable.ClassBusinessNome + " obj" + sqlTable.ClassBusinessNome + ")");
                builder.AppendLine(Tab(2) + "{");
                builder.AppendLine(Tab(3) + "if (obj" + sqlTable.ClassBusinessNome + "._" + sqlTable.GetIdentityColumn().Name + " == null) return null;");
                builder.AppendLine(Tab(3) + "DataBase." + sqlTable.ClassBusinessNome + " db = new DataBase." + sqlTable.ClassBusinessNome + "();");
                builder.AppendLine(Tab(3) + "try");
                builder.AppendLine(Tab(3) + "{");
                builder.AppendLine(Tab(4) + "DbDataReader dr = db.Get(" + GetInsertParams("obj" + sqlTable.ClassBusinessNome, false) + ");");
                builder.AppendLine(Tab(4) + "if (dr != null && dr.Read()) return setObject(dr, obj" + sqlTable.ClassBusinessNome + ");");
                builder.AppendLine(Tab(4) + "else return null;");
                builder.AppendLine(Tab(3) + "}");
                builder.AppendLine(Tab(3) + "catch { return null; }");
                builder.AppendLine(Tab(3) + "finally { db.CloseConnection(); }");
                builder.AppendLine(Tab(2) + "}");
                builder.AppendLine();
                builder.AppendLine(Tab(2) + "/// <summary>");
                builder.AppendLine(Tab(2) + "/// Retorna " + sqlTable.ClassBusinessNome.ToLower() + " pelo id");
                builder.AppendLine(Tab(2) + "/// </summary>");
                builder.AppendLine(Tab(2) + "/// <param name=\"id\"> id do " + sqlTable.ClassBusinessNome.ToLower() + " </param>");
                builder.AppendLine(Tab(2) + "public static " + sqlTable.ClassBusinessNome + " GetById(" + sqlTable.GetIdentityColumn().NetDataType + " id){ return getByObject(new " + sqlTable.ClassBusinessNome + "(id,true)); }");
                builder.AppendLine();
                builder.AppendLine(Tab(2) + "/// <summary>");
                builder.AppendLine(Tab(2) + "/// Retorna lista de " + sqlTable.ClassBusinessNome.ToLower() + "");
                builder.AppendLine(Tab(2) + "/// </summary>");
                builder.AppendLine(Tab(2) + "public static L" + sqlTable.ClassBusinessNome + " GetAll(){ return GetByParameters(" + GerarNulls(sqlTable.Colunas.Count) + "); }");
                builder.AppendLine();
                builder.AppendLine(Tab(2) + "/// <summary>");
                builder.AppendLine(Tab(2) + "/// Retorna lista de " + sqlTable.ClassBusinessNome.ToLower() + " filtrando com par\x00e2metros");
                builder.AppendLine(Tab(2) + "/// Ex: usar no ObjectDataSource");
                builder.AppendLine(Tab(2) + "/// </summary>");
                builder.AppendLine(Tab(2) + "public static L" + sqlTable.ClassBusinessNome + " GetByParameters(" + sqlTable.GetColumnsInParams() + ")");
                builder.AppendLine(Tab(2) + "{");
                builder.AppendLine(Tab(3) + Program.prefixNamespace + "DataBase." + sqlTable.ClassDataBaseNome + " db = new " + Program.prefixNamespace + "DataBase." + sqlTable.ClassDataBaseNome + "();");
                builder.AppendLine(Tab(3) + "try");
                builder.AppendLine(Tab(3) + "{");
                builder.AppendLine(Tab(4) + "L" + sqlTable.ClassBusinessNome + " L" + sqlTable.ClassBusinessNome + " = new L" + sqlTable.ClassBusinessNome + "();");
                builder.AppendLine(Tab(4) + "DbDataReader dr = db.Get(" + sqlTable.GetColumnsOutParams() + ");");
                builder.AppendLine(Tab(4) + "if (dr != null)");
                builder.AppendLine(Tab(5) + "while (dr.Read()) L" + sqlTable.ClassDataBaseNome + ".Add(setObject(dr, new " + sqlTable.ClassDataBaseNome + "()));");
                builder.AppendLine(Tab(4) + "return L" + sqlTable.ClassDataBaseNome + ";");
                builder.AppendLine(Tab(3) + "}");
                builder.AppendLine(Tab(3) + "catch{ return null; }");
                builder.AppendLine(Tab(3) + "finally{ db.CloseConnection(); }");
                builder.AppendLine(Tab(2) + "}");
                builder.AppendLine();
                string str2 = "int?";
                if (sqlTable.GetIdentityColumn().NetDataType == "decimal?")
                {
                    str2 = "decimal?";
                }
                builder.AppendLine(Tab(2) + "/// <summary>");
                builder.AppendLine(Tab(2) + "/// Salva o registro atrav\x00e9s de uma refer\x00eancia do objeto " + sqlTable.ClassBusinessNome + "");
                builder.AppendLine(Tab(2) + "/// Ex: usar no ObjectDataSource");
                builder.AppendLine(Tab(2) + "/// </summary>");
                builder.AppendLine(Tab(2) + "/// <param name=\"obj" + sqlTable.ClassBusinessNome + "\">objeto " + sqlTable.ClassBusinessNome.ToLower() + "</param>");
                builder.AppendLine(Tab(2) + "public static " + str2 + " Save(" + sqlTable.ClassBusinessNome + " obj" + sqlTable.ClassBusinessNome + ")");
                builder.AppendLine(Tab(2) + "{");
                builder.AppendLine(Tab(3) + "try");
                builder.AppendLine(Tab(3) + "{");
                builder.AppendLine(Tab(4) + Program.prefixNamespace + "DataBase." + sqlTable.ClassDataBaseNome + " db = new " + Program.prefixNamespace + "DataBase." + sqlTable.ClassDataBaseNome + "();");
                builder.AppendLine(Tab(4) + "setTransaction(obj" + sqlTable.ClassBusinessNome + ", db);");
                builder.AppendLine(Tab(4) + "obj" + sqlTable.ClassBusinessNome + ".Transaction = db.Save(" + GetInsertParams("obj" + sqlTable.ClassBusinessNome, true) + ");");
                builder.AppendLine(Tab(4) + "return obj" + sqlTable.ClassBusinessNome + "._" + sqlTable.GetIdentityColumn().Name + ";");
                builder.AppendLine(Tab(3) + "}");
                builder.AppendLine(Tab(3) + "catch(Exception ex){ throw ex; }");
                builder.AppendLine(Tab(2) + "}");
                builder.AppendLine();
                builder.AppendLine(Tab(2) + "/// <summary>");
                builder.AppendLine(Tab(2) + "/// Exclui o registro atrav\x00e9s de uma refer\x00eancia do objeto " + sqlTable.ClassBusinessNome.ToLower() + "");
                builder.AppendLine(Tab(2) + "/// Ex: usar no ObjectDataSource");
                builder.AppendLine(Tab(2) + "/// </summary>");
                builder.AppendLine(Tab(2) + "/// <param name=\"obj" + sqlTable.ClassBusinessNome + "\">objeto " + sqlTable.ClassBusinessNome.ToLower() + "</param>");
                builder.AppendLine(Tab(2) + "public static void Delete(" + sqlTable.ClassBusinessNome + " obj" + sqlTable.ClassBusinessNome + ")");
                builder.AppendLine(Tab(2) + "{");
                builder.AppendLine(Tab(3) + "try");
                builder.AppendLine(Tab(3) + "{");
                builder.AppendLine(Tab(4) + Program.prefixNamespace + "DataBase." + sqlTable.ClassDataBaseNome + " db = new " + Program.prefixNamespace + "DataBase." + sqlTable.ClassDataBaseNome + "();");
                builder.AppendLine(Tab(4) + "setTransaction(obj" + sqlTable.ClassBusinessNome + ", db);");
                builder.AppendLine(Tab(4) + "obj" + sqlTable.ClassBusinessNome + ".Transaction = db.Delete(" + GetInsertParams("obj" + sqlTable.ClassBusinessNome, false) + ");");
                builder.AppendLine(Tab(3) + "}");
                builder.AppendLine(Tab(3) + "catch(Exception ex){ throw ex; }");
                builder.AppendLine(Tab(2) + "}");
            }
            if (sqlTable.IsTableNo)
            {
                int num = 0;
                foreach (SqlColumn column2 in sqlTable.Colunas)
                {
                    List<SqlReference> referencesForFk = column2.GetReferencesForFk(sqlTable);
                    SqlColumn column3 = null;
                    if (num == 0)
                    {
                        column3 = sqlTable.GetColumn(sqlTable.Colunas[1].Name);
                    }
                    else
                    {
                        column3 = sqlTable.GetColumn(sqlTable.Colunas[0].Name);
                    }
                    string[] strArray = new string[11];
                    strArray[0] = Tab(2);
                    strArray[1] = "/// <summary>\n";
                    strArray[1] = "/// Retorna lista de L" + referencesForFk[0].ClassBusinessName + "\n";
                    strArray[1] = "/// </summary>\n";
                    builder.AppendLine(Tab(2) + "/// <param name=\"" + column3.Name + "\">Seta par\x00e2metro " + column3.Name + "</param>");
                    strArray[1] = "public static L";
                    strArray[2] = referencesForFk[0].ClassBusinessName;
                    strArray[3] = " Get";
                    char ch = referencesForFk[0].ClassBusinessName[0];
                    strArray[4] = ch.ToString().ToUpper();
                    strArray[5] = referencesForFk[0].ClassBusinessName.Substring(1);
                    strArray[6] = "(";
                    strArray[7] = column3.NetDataType;
                    strArray[8] = " ";
                    strArray[9] = column3.Name;
                    strArray[10] = "_)";
                    builder.AppendLine(string.Concat(strArray));
                    builder.AppendLine(Tab(2) + "{");
                    builder.AppendLine(Tab(3) + Program.prefixNamespace + "DataBase." + sqlTable.ClassDataBaseNome + " db = new " + Program.prefixNamespace + "DataBase." + sqlTable.ClassDataBaseNome + "();");
                    builder.AppendLine(Tab(3) + "try");
                    builder.AppendLine(Tab(3) + "{");
                    builder.AppendLine(Tab(4) + "L" + referencesForFk[0].ClassBusinessName + " L" + referencesForFk[0].ClassBusinessName.ToLower() + " = new L" + referencesForFk[0].ClassBusinessName + "();");
                    string[] strArray2 = new string[7];
                    strArray2[0] = Tab(4);
                    strArray2[1] = "DbDataReader dr = db.Get_";
                    char ch2 = referencesForFk[0].ClassBusinessName[0];
                    strArray2[2] = ch2.ToString().ToUpper();
                    strArray2[3] = referencesForFk[0].ClassBusinessName.Substring(1);
                    strArray2[4] = "(";
                    strArray2[5] = column3.Name;
                    strArray2[6] = "_);";
                    builder.AppendLine(string.Concat(strArray2));
                    builder.AppendLine(Tab(4) + "if (dr != null)");
                    builder.AppendLine(Tab(5) + "while (dr.Read()) L" + referencesForFk[0].ClassBusinessName.ToLower() + ".Add(" + referencesForFk[0].ClassBusinessName + ".setObject(dr, new " + referencesForFk[0].ClassBusinessName + "()));");
                    builder.AppendLine(Tab(4) + "return L" + referencesForFk[0].ClassBusinessName.ToLower() + ";");
                    builder.AppendLine(Tab(3) + "}");
                    builder.AppendLine(Tab(3) + "catch{ return null; }");
                    builder.AppendLine(Tab(3) + "finally{ db.CloseConnection(); }");
                    builder.AppendLine(Tab(2) + "}");
                    num++;
                    builder.AppendLine();
                }
            }
            builder.AppendLine(Tab(2) + "#endregion");
            return builder.ToString();
        }

        private static string GerarMetodos(SqlTable sqlTable)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(Tab(2) + "#region Metodos");
            if (!sqlTable.IsTableNo)
            {
                builder.AppendLine(Tab(2) + "/// <summary>");
                builder.AppendLine(Tab(2) + "/// Seta o objeto atual com os registros do banco");
                builder.AppendLine(Tab(2) + "/// </summary>");
                builder.AppendLine(Tab(2) + "public void Get(){ getByObject(this); }");
                builder.AppendLine();
                string str = "int?";
                if (sqlTable.GetIdentityColumn().NetDataType == "decimal?")
                {
                    str = "decimal?";
                }
                builder.AppendLine(Tab(2) + "/// <summary>");
                builder.AppendLine(Tab(2) + "/// Salva o objeto atual");
                builder.AppendLine(Tab(2) + "/// </summary>");
                builder.AppendLine(Tab(2) + "public " + str + " Save(){ return Save(this); }");
                builder.AppendLine();
                builder.AppendLine(Tab(2) + "/// <summary>");
                builder.AppendLine(Tab(2) + "/// Exclui o objeto atual");
                builder.AppendLine(Tab(2) + "/// </summary>");
                builder.AppendLine(Tab(2) + "public void Delete(){ Delete(this); }");
            }
            else
            {
                string[] strArray;
                char ch;
                builder.AppendLine(Tab(2) + "/// <summary>");
                builder.AppendLine(Tab(2) + "/// Salva o objeto atual");
                builder.AppendLine(Tab(2) + "/// </summary>");
                builder.AppendLine(Tab(2) + "public void Save()");
                builder.AppendLine(Tab(2) + "{");
                builder.AppendLine(Tab(3) + "try");
                builder.AppendLine(Tab(3) + "{");
                builder.AppendLine(Tab(4) + Program.prefixNamespace + "DataBase." + sqlTable.ClassDataBaseNome + " db = new " + Program.prefixNamespace + "DataBase." + sqlTable.ClassDataBaseNome + "();");
                builder.AppendLine(Tab(4) + "setTransaction(this, db);");
                if (!sqlTable.IsTableNo)
                {
                    builder.AppendLine(Tab(4) + "Transaction = db.Save(" + GetInsertParams("", false) + ");");
                    builder.AppendLine(Tab(4) + "return this._" + sqlTable.GetIdentityColumn().Name + ";");
                }
                else
                {
                    strArray = new string[12];
                    strArray[0] = Tab(4);
                    strArray[1] = "Transaction = db.Save(R";
                    strArray[2] = getReference(sqlTable.References, sqlTable.Colunas[0].Name);
                    strArray[3] = ".";
                    ch = sqlTable.Colunas[0].Name[0];
                    strArray[4] = ch.ToString().ToUpper();
                    strArray[5] = sqlTable.Colunas[0].Name.Substring(1);
                    strArray[6] = ", R";
                    strArray[7] = getReference(sqlTable.References, sqlTable.Colunas[1].Name);
                    strArray[8] = ".";
                    ch = sqlTable.Colunas[1].Name[0];
                    strArray[9] = ch.ToString().ToUpper();
                    strArray[10] = sqlTable.Colunas[1].Name.Substring(1);
                    strArray[11] = ");";
                    builder.AppendLine(string.Concat(strArray));
                }
                builder.AppendLine(Tab(3) + "}");
                builder.AppendLine(Tab(3) + "catch(Exception ex){ throw ex; }");
                builder.AppendLine(Tab(2) + "}");
                builder.AppendLine();
                builder.AppendLine(Tab(2) + "/// <summary>");
                builder.AppendLine(Tab(2) + "/// Exclui o objeto atual");
                builder.AppendLine(Tab(2) + "/// </summary>");
                builder.AppendLine(Tab(2) + "public void Delete()");
                builder.AppendLine(Tab(2) + "{");
                builder.AppendLine(Tab(3) + "try");
                builder.AppendLine(Tab(3) + "{");
                builder.AppendLine(Tab(4) + Program.prefixNamespace + "DataBase." + sqlTable.ClassDataBaseNome + " db = new " + Program.prefixNamespace + "DataBase." + sqlTable.ClassDataBaseNome + "();");
                builder.AppendLine(Tab(4) + "setTransaction(this, db);");
                if (!sqlTable.IsTableNo)
                {
                    builder.AppendLine(Tab(4) + "Transaction = db.Delete(" + GetInsertParams("", false) + ");");
                }
                else
                {
                    strArray = new string[12];
                    strArray[0] = Tab(4);
                    strArray[1] = "Transaction = db.Delete(R";
                    strArray[2] = getReference(sqlTable.References, sqlTable.Colunas[0].Name);
                    strArray[3] = ".";
                    ch = sqlTable.Colunas[0].Name[0];
                    strArray[4] = ch.ToString().ToUpper();
                    strArray[5] = sqlTable.Colunas[0].Name.Substring(1);
                    strArray[6] = ", R";
                    strArray[7] = getReference(sqlTable.References, sqlTable.Colunas[1].Name);
                    strArray[8] = ".";
                    ch = sqlTable.Colunas[1].Name[0];
                    strArray[9] = ch.ToString().ToUpper();
                    strArray[10] = sqlTable.Colunas[1].Name.Substring(1);
                    strArray[11] = ");";
                    builder.AppendLine(string.Concat(strArray));
                }
                builder.AppendLine(Tab(3) + "}");
                builder.AppendLine(Tab(3) + "catch(Exception ex){ throw ex; }");
                builder.AppendLine(Tab(2) + "}");
            }
            builder.AppendLine();
            builder.AppendLine(Tab(2) + "#endregion");
            return builder.ToString();
        }

        private static string GerarNulls(int qtde)
        {
            string str = "";
            for (int i = 0; i < qtde; i++)
            {
                str = str + "null, ";
            }
            if (str != "")
            {
                str = str.Remove(str.Length - 2, 2);
            }
            return str;
        }

        private static void GerarParametros()
        {
            StringBuilder builder = new StringBuilder();
            StringBuilder builder2 = new StringBuilder();
            foreach (AtributesForClass class2 in camposClass)
            {
                if ((class2.IniName == "") || (class2.IniName == "R"))
                {
                    builder.Append(class2.IniList + class2.Tipo + class2.FimList + " " + class2.IniName + class2.ClearName + "_, ");
                    builder2.Append(class2.IniName + class2.ClearName + ",");
                }
            }
            parametros = builder.ToString();
            parametros = parametros + "bool isFull_";
            parametrosLimp = builder2.ToString();
            parametrosLimp = parametrosLimp + "isFull";
        }

        private static string GerarPropriedades(SqlTable sqlTable)
        {
            sc.Clear();
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(Tab(2) + "#region Propriedades");
            builder.AppendLine(Tab(2) + "/// <summary>");
            builder.AppendLine(Tab(2) + "/// Seta ou Repassa a trasa\x00e7\x00e3o para os outros objetos");
            builder.AppendLine(Tab(2) + "/// </summary>");
            builder.AppendLine(Tab(2) + "public DbTransaction Transaction");
            builder.AppendLine(Tab(2) + "{");
            builder.AppendLine(Tab(3) + "get { return this._transaction; }");
            builder.AppendLine(Tab(3) + "set { this._transaction = value; }");
            builder.AppendLine(Tab(2) + "}");
            builder.AppendLine(Tab(2) + "/// <summary>");
            builder.AppendLine(Tab(2) + "/// Inicia a transa\x00e7\x00e3o no objeto, ao utilizar sempre finalize com Transaction.Commit ou Transaction.Rollback");
            builder.AppendLine(Tab(2) + "/// </summary>");
            builder.AppendLine(Tab(2) + "public bool IsTransaction");
            builder.AppendLine(Tab(2) + "{");
            builder.AppendLine(Tab(3) + "get { return this._isTransaction; }");
            builder.AppendLine(Tab(3) + "set { this._isTransaction = value; }");
            builder.AppendLine(Tab(2) + "}");
            builder.AppendLine(Tab(2) + "/// <summary>");
            builder.AppendLine(Tab(2) + "/// Verifica se o objeto est\x00e1 preenchido, false: preenche o objeto com os dados da base, true: Utiliza o objeto atual");
            builder.AppendLine(Tab(2) + "/// </summary>");
            builder.AppendLine(Tab(2) + "public bool IsFull { get { return this._isFull; } }");
            foreach (AtributesForClass class2 in camposClass)
            {
                string[] strArray6;
                string str4;
                if (class2.IniName == "")
                {
                    builder.AppendLine(Tab(2) + "/// <summary>");
                    builder.AppendLine(Tab(2) + "/// Seta ou retorna a propriedade " + class2.PropertiesName + "");
                    builder.AppendLine(Tab(2) + "/// </summary>");
                    builder.AppendLine(Tab(2) + "public " + class2.PropertiesType + " " + class2.PropertiesName);
                    builder.AppendLine(Tab(2) + "{");
                    builder.AppendLine(Tab(3) + "get{ return this._" + class2.IniName + class2.ClearName + ";}");
                    builder.AppendLine(Tab(3) + "set{ this._" + class2.IniName + class2.ClearName + " = value;}");
                    builder.AppendLine(Tab(2) + "}");
                    continue;
                }
                if (class2.IniName == "R")
                {
                    SqlTable table = SqlTable.GetTable(class2.ClearName);
                    char ch7 = table.GetIdentityColumn().Name[0];
                    string str = ch7.ToString().ToUpper() + table.GetIdentityColumn().Name.Substring(1);
                    builder.AppendLine(Tab(2) + "/// <summary>");
                    builder.AppendLine(Tab(2) + "/// Seta e um novo objeto " + class2.ClearName.ToLower() + " para propriedade " + str + "");
                    builder.AppendLine(Tab(2) + "/// Ex: usar no Bind() do ObjectDataSource para opera\x00e7\x00f5es de save, delete");
                    builder.AppendLine(Tab(2) + "/// </summary>");
                    builder.AppendLine(Tab(2) + "public " + table.GetIdentityColumn().SqlDataType + "? " + str + "");
                    builder.AppendLine(Tab(2) + "{");
                    builder.AppendLine(Tab(3) + "get{ return this.R" + class2.ClearName + "." + str + "; }");
                    builder.AppendLine(Tab(3) + "set{ _R" + class2.ClearName + " = new " + class2.ClearName + "(value,true); }");
                    builder.AppendLine(Tab(2) + "}");
                    builder.AppendLine(Tab(2) + "/// <summary>");
                    builder.AppendLine(Tab(2) + "/// Seta ou retorna a refer\x00eancia do " + class2.ClearName.ToLower() + "");
                    builder.AppendLine(Tab(2) + "/// </summary>");
                    builder.AppendLine(Tab(2) + "public " + class2.PropertiesType + " " + class2.PropertiesName);
                    builder.AppendLine(Tab(2) + "{");
                    builder.AppendLine(Tab(3) + "get");
                    builder.AppendLine(Tab(3) + "{");
                    SqlTable.GetTable(class2.ClearName).GetIdentityColumn();
                    string[] strArray = new string[] { Tab(4), "if (this._" + class2.IniName + class2.ClearName + " == null)\n\t\t\t\t{\n\t\t\t\t\tthis._" + class2.IniName + class2.ClearName + " = new " + class2.ClearName + "();\n\t\t\t\t\treturn this._" + class2.IniName + class2.ClearName + ";\n\t\t\t\t}\n", "\t\t\t\tif (!this._" + class2.IniName + class2.ClearName + ".IsFull) this._" + class2.IniName + class2.ClearName + ".Get();\n" };
                    str4 = strArray6[2];
                    (strArray6 = strArray)[2] = str4 + "\t\t\t\treturn this._" + class2.IniName + class2.ClearName + ";";
                    builder.AppendLine(string.Concat(strArray));
                    builder.AppendLine(Tab(3) + "}");
                    builder.AppendLine(Tab(3) + "set{ this._" + class2.IniName + class2.ClearName + " = value;}");
                    builder.AppendLine(Tab(2) + "}");
                    continue;
                }
                builder.AppendLine(Tab(2) + "/// <summary>");
                builder.AppendLine(Tab(2) + "/// Seta ou retorna a cole\x00e7\x00e3o do objeto " + class2.PropertiesType + "");
                builder.AppendLine(Tab(2) + "/// </summary>");
                builder.AppendLine(Tab(2) + "public " + class2.PropertiesType + " " + class2.PropertiesType);
                builder.AppendLine(Tab(2) + "{");
                builder.AppendLine(Tab(3) + "get");
                builder.AppendLine(Tab(3) + "{");
                SqlTable.GetTable(class2.ClearName).GetIdentityColumn();
                char ch = class2.ClearName[0];
                string referenceObject = ch.ToString().ToUpper() + class2.ClearName.Substring(1);
                if (!sqlTable.IsTableNo)
                {
                    SqlTable tableNo = GetTableNo(sqlTable, class2);
                    if (tableNo == null)
                    {
                        string[] strArray2 = new string[7];
                        strArray2[0] = Tab(4);
                        strArray2[1] = "if (this._" + class2.IniName + referenceObject + " == null) this._" + class2.IniName + referenceObject + " = ";
                        char ch2 = class2.ClearName[0];
                        strArray2[2] = ch2.ToString().ToUpper();
                        strArray2[3] = class2.ClearName.Substring(1);
                        strArray2[4] = ".GetByParameters(";
                        strArray2[5] = GetSelectParams(sqlTable, referenceObject);
                        strArray2[6] = ");";
                        str4 = strArray6[6];
                        (strArray6 = strArray2)[6] = str4 + "\n" + Tab(4) + "return this._" + class2.IniName + referenceObject + ";";
                        builder.AppendLine(string.Concat(strArray2));
                    }
                    else
                    {
                        SqlTable tableToNo = GetTableToNo(sqlTable, tableNo);
                        if ((tableToNo != null) && (tableToNo.ClassBusinessNome != class2.Tipo))
                        {
                            string[] strArray3 = new string[7];
                            strArray3[0] = Tab(4);
                            strArray3[1] = "if (this._" + class2.IniName + referenceObject + " == null) this._" + class2.IniName + referenceObject + " = ";
                            char ch3 = class2.ClearName[0];
                            strArray3[2] = ch3.ToString().ToUpper();
                            strArray3[3] = class2.ClearName.Substring(1);
                            strArray3[4] = ".GetByParameters(";
                            strArray3[5] = GetSelectParams(sqlTable, referenceObject);
                            strArray3[6] = ");";
                            str4 = strArray6[6];
                            (strArray6 = strArray3)[6] = str4 + "\n" + Tab(4) + "return this._" + class2.IniName + referenceObject + ";";
                            builder.AppendLine(string.Concat(strArray3));
                        }
                        else
                        {
                            SqlTable table4 = GetTableToNo(sqlTable, tableNo);
                            SqlColumn identityColumn = sqlTable.GetIdentityColumn();
                            string[] strArray4 = new string[9];
                            strArray4[0] = Tab(4);
                            strArray4[1] = "if (this._" + class2.IniName + referenceObject + " == null) this._" + class2.IniName + referenceObject + " = ";
                            strArray4[2] = tableNo.ClassBusinessNome;
                            strArray4[3] = ".Get";
                            strArray4[4] = table4.ClassBusinessNome;
                            strArray4[5] = "(this.";
                            char ch4 = identityColumn.Name[0];
                            strArray4[6] = ch4.ToString().ToUpper();
                            strArray4[7] = identityColumn.Name.Substring(1);
                            strArray4[8] = ");";
                            str4 = strArray6[8];
                            (strArray6 = strArray4)[8] = str4 + "\n" + Tab(4) + "return this._" + class2.IniName + referenceObject + ";";
                            builder.AppendLine(string.Concat(strArray4));
                        }
                    }
                }
                else
                {
                    AtributesForClass atributo = AtributesForClass.GetAtributo(class2.PropertiesName);
                    if (atributo != null)
                    {
                        string[] strArray5 = new string[10];
                        strArray5[0] = Tab(4);
                        strArray5[1] = "if (this._" + class2.IniName + referenceObject + " == null) this._" + class2.IniName + referenceObject + " = ";
                        strArray5[2] = sqlTable.ClassBusinessNome;
                        strArray5[3] = ".Get";
                        char ch5 = class2.ClearName[0];
                        strArray5[4] = ch5.ToString().ToUpper();
                        strArray5[5] = class2.ClearName.Substring(1);
                        strArray5[6] = "(R" + sqlTable.ClassBusinessNome.Replace(referenceObject, "") + ".";
                        char ch6 = atributo.AtributeRef[0];
                        strArray5[7] = ch6.ToString().ToUpper();
                        strArray5[8] = atributo.AtributeRef.Substring(1);
                        strArray5[9] = ");";
                        str4 = strArray6[9];
                        (strArray6 = strArray5)[9] = str4 + "\n" + Tab(4) + "return this._" + class2.IniName + referenceObject + ";";
                        builder.AppendLine(string.Concat(strArray5));
                    }
                }
                builder.AppendLine(Tab(3) + "}");
                builder.AppendLine(Tab(3) + "set{ this._" + class2.IniName + referenceObject + " = value;}");
                builder.AppendLine(Tab(2) + "}");
            }
            builder.AppendLine(Tab(2) + "#endregion");
            return builder.ToString();
        }

        private static string GetInsertParams(string objName, bool refs)
        {
            StringBuilder builder = new StringBuilder();
            foreach (AtributesForClass class2 in camposClass)
            {
                if ((class2.TipoKey != "FK") && (class2.TipoKey != "UK"))
                {
                    if (((class2.TipoKey == "PK") && class2.IsIdentity) || (class2.TipoKey == ""))
                    {
                        if (objName == "")
                        {
                            builder.Append("this._" + class2.ClearName + ", ");
                        }
                        else if (class2.IsIdentity && refs)
                        {
                            builder.Append("ref " + objName + "._" + class2.ClearName + ", ");
                        }
                        else
                        {
                            builder.Append(objName + "._" + class2.ClearName + ", ");
                        }
                    }
                    continue;
                }
                if (class2.IniName != "")
                {
                    SqlColumn identityColumn = SqlTable.GetTable(class2.ClearName).GetIdentityColumn();
                    string[] strArray = new string[6];
                    if (objName == "")
                    {
                        strArray[0] = "this.R";
                    }
                    else
                    {
                        strArray[0] = objName + ".R";
                    }
                    strArray[1] = class2.ClearName;
                    strArray[2] = ".";
                    char ch = identityColumn.Name[0];
                    strArray[3] = ch.ToString().ToUpper();
                    strArray[4] = identityColumn.Name.Substring(1);
                    strArray[5] = ", ";
                    builder.Append(string.Concat(strArray));
                }
            }
            if (builder.Length > 2)
            {
                builder.Remove(builder.Length - 2, 2);
            }
            return builder.ToString();
        }

        private static string GetParamsNull()
        {
            string str = "";
            foreach (AtributesForClass class2 in camposClass)
            {
                if (class2.IniList == null)
                {
                    str = str + "null, ";
                }
            }
            return str.Substring(0, str.Length - 8);
        }

        private static string GetParamsNulls(int qtdeRemove)
        {
            int num = camposClass.Count - qtdeRemove;
            string str = "";
            for (int i = 0; i < num; i++)
            {
                str = str + "null, ";
            }
            if (str != "")
            {
                str = str.Remove(str.Length - 2, 2);
            }
            return str;
        }

        private static string getReference(List<SqlReference> refs, string colunName)
        {
            foreach (SqlReference reference in refs)
            {
                if (reference.PkColumnName == colunName)
                {
                    return reference.ClassBusinessName;
                }
            }
            return "";
        }

        private static string GetSelectById(SqlTable actualTable, string referenceObject)
        {
            StringBuilder builder = new StringBuilder();
            actualTable.GetIdentityColumn();
            foreach (SqlColumn column in actualTable.Colunas)
            {
                if (column.IsFk)
                {
                    foreach (SqlReference reference in actualTable.References)
                    {
                        if (((referenceObject.ToLower() == reference.ClassBusinessName.ToLower()) && (reference.PkColumnName.ToLower() == column.Name.ToLower())) && (actualTable.Nome.ToLower() == reference.FkTable.ToLower()))
                        {
                            char ch = reference.PkColumnName[0];
                            builder.Append("this." + ch.ToString().ToUpper() + reference.PkColumnName.Substring(1) + ", ");
                        }
                    }
                }
            }
            if (builder.Length > 2)
            {
                builder.Remove(builder.Length - 2, 2);
            }
            return builder.ToString();
        }

        private static string GetSelectParams(SqlTable actualTable, string referenceObject)
        {
            StringBuilder builder = new StringBuilder();
            SqlTable table = SqlTable.GetTable(referenceObject);
            SqlColumn identityColumn = actualTable.GetIdentityColumn();
            bool flag = false;
            foreach (SqlColumn column2 in table.Colunas)
            {
                if (column2.IsFk)
                {
                    foreach (SqlReference reference in table.References)
                    {
                        if ((reference.PkTable.ToLower() == actualTable.Nome.ToLower()) && (reference.PkColumnName.ToLower() == column2.Name.ToLower()))
                        {
                            builder.Append("this._" + identityColumn.Name + ", ");
                            flag = true;
                            break;
                        }
                    }
                    if (flag)
                    {
                        flag = false;
                    }
                    else
                    {
                        builder.Append("null, ");
                    }
                    continue;
                }
                builder.Append("null, ");
            }
            if (builder.Length > 2)
            {
                builder.Remove(builder.Length - 2, 2);
            }
            return builder.ToString();
        }

        private static SqlTable GetTableNo(SqlTable table, AtributesForClass clearName)
        {
            foreach (SqlReference reference in table.References)
            {
                SqlTable table2 = SqlTable.GetTable(reference.FkTable);
                if ((table2 != null) && table2.IsTableNo)
                {
                    foreach (SqlReference reference2 in table2.References)
                    {
                        SqlTable table3 = SqlTable.GetTable(reference2.PkTable);
                        if (!((!(table3.ClassBusinessNome.ToLower() == clearName.ClearName.ToLower()) || !(table3.ClassBusinessNome.ToLower() != table.ClassBusinessNome.ToLower())) || sc.Contains(table2.Nome)))
                        {
                            sc.Add(table2.Nome);
                            return table2;
                        }
                    }
                }
            }
            return null;
        }

        private static SqlTable GetTableNo2(SqlTable table, AtributesForClass clearName)
        {
            if (Program.SqlTableList != null)
            {
                foreach (SqlTable table2 in Program.SqlTableList)
                {
                    if (table2.IsTableNo)
                    {
                        foreach (SqlReference reference in table2.References)
                        {
                            if (!(!(reference.PkTable.ToLower() == table.Nome.ToLower()) || sc.Contains(table.Nome.ToLower())))
                            {
                                sc.Add(table.Nome.ToLower());
                                return table2;
                            }
                        }
                    }
                }
            }
            return null;
        }

        private static SqlTable GetTableToNo(SqlTable sqlTable, SqlTable sqlTableNo)
        {
            if ((Program.SqlTableList != null) && (Program.SqlTableList.Count > 0))
            {
                foreach (SqlTable table in Program.SqlTableList)
                {
                    foreach (SqlReference reference in table.References)
                    {
                        if (((reference.FkTable.ToLower() == sqlTableNo.Nome.ToLower()) && (reference.PkTable.ToLower() != sqlTable.Nome.ToLower())) && (table.Nome.ToLower() != sqlTableNo.Nome.ToLower()))
                        {
                            return table;
                        }
                    }
                }
            }
            return null;
        }

        private static string Tab(int qtde)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < qtde; i++)
            {
                builder.Append("\t");
            }
            return builder.ToString();
        }

        public class AtributesForClass
        {
            private string _clearName;
            private string _fimList;
            private string _iniList;
            private string _iniName;
            private bool _isCollection;
            private bool _isIdentity;
            private string _propertiesName;
            private string _propertiesType;
            private string _tipo;
            private string _tipoKey;
            private string atributeRef;

            public AtributesForClass(string tipo_, string clearName_, bool isCollection_, string iniName_, string tipoKey_, bool isIdentity_, string atributeRef_)
            {
                this._tipo = tipo_;
                this._clearName = clearName_;
                this._isCollection = isCollection_;
                this.IniName = iniName_;
                this._tipoKey = tipoKey_;
                this._isIdentity = isIdentity_;
                this.atributeRef = atributeRef_;
                if (isCollection_ && (iniName_ == "L"))
                {
                    this._iniList = "";
                    this._fimList = "L";
                    this._propertiesType = "L" + tipo_ + "";
                    char ch = iniName_[0];
                    this.PropertiesName = ch.ToString().ToUpper() + iniName_.Substring(1) + clearName_;
                }
                else if (iniName_ == "R")
                {
                    this._iniList = "";
                    this._fimList = "";
                    this._propertiesType = tipo_;
                    char ch2 = iniName_[0];
                    this.PropertiesName = ch2.ToString().ToUpper() + iniName_.Substring(1) + clearName_;
                }
                else
                {
                    this._propertiesType = tipo_;
                    char ch3 = clearName_[0];
                    this.PropertiesName = ch3.ToString().ToUpper() + clearName_.Substring(1);
                }
            }

            public static GerarBusinessClass.AtributesForClass GetAtributo(string propertiesName)
            {
                if ((GerarBusinessClass.camposClass != null) && (GerarBusinessClass.camposClass.Count > 0))
                {
                    foreach (GerarBusinessClass.AtributesForClass class2 in GerarBusinessClass.camposClass)
                    {
                        if (propertiesName.ToLower() == class2.PropertiesName.ToLower())
                        {
                            return class2;
                        }
                    }
                }
                return null;
            }

            public string AtributeRef
            {
                get
                {
                    return this.atributeRef;
                }
                set
                {
                    this.atributeRef = value;
                }
            }

            public string ClearName
            {
                get
                {
                    return this._clearName;
                }
                set
                {
                    this._clearName = value;
                }
            }

            public string FimList
            {
                get
                {
                    return this._fimList;
                }
                set
                {
                    this._fimList = value;
                }
            }

            public string IniList
            {
                get
                {
                    return this._iniList;
                }
                set
                {
                    this._iniList = value;
                }
            }

            public string IniName
            {
                get
                {
                    return this._iniName;
                }
                set
                {
                    this._iniName = value;
                }
            }

            public bool IsCollection
            {
                get
                {
                    return this._isCollection;
                }
                set
                {
                    this._isCollection = value;
                }
            }

            public bool IsIdentity
            {
                get
                {
                    return this._isIdentity;
                }
                set
                {
                    this._isIdentity = value;
                }
            }

            public string PropertiesName
            {
                get
                {
                    return this._propertiesName;
                }
                set
                {
                    this._propertiesName = value;
                }
            }

            public string PropertiesType
            {
                get
                {
                    return this._propertiesType;
                }
                set
                {
                    this._propertiesType = value;
                }
            }

            public string Tipo
            {
                get
                {
                    return this._tipo;
                }
                set
                {
                    this._tipo = value;
                }
            }

            public string TipoKey
            {
                get
                {
                    return this._tipoKey;
                }
                set
                {
                    this._tipoKey = value;
                }
            }
        }
    }
}

