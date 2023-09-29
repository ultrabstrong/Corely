using System.Xml;
using System.Xml.Serialization;

namespace Corely.Shared.Mappers.Liquibase.EntityMappers
{
    public sealed class EntityToLiquibaseXmlMapper : EntityToLiquibaseMapperBase
    {
        public EntityToLiquibaseXmlMapper(string assemblyPath, string rootEntityNamespace)
            : base(assemblyPath, rootEntityNamespace)
        {
            throw new NotImplementedException("Current XML implentation is placeholder and does not work for Liquibase");
        }

        public override string Map()
        {
            var changeLog = MapDatabaseChangeLog();

            XmlSerializer xmlSerializer = new(changeLog.GetType());
            XmlWriterSettings xmlSettings = new() { Indent = true };

            using StringWriter stringWriter = new();
            using var xmlWriter = XmlWriter.Create(stringWriter, xmlSettings);

            xmlSerializer.Serialize(xmlWriter, changeLog);
            var changeLogXml = stringWriter.ToString();

            return changeLogXml;
        }
    }
}
