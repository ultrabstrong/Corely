using System.Xml;
using System.Xml.Serialization;

namespace Corely.Shared.Mappers.Liquibase
{
    public class EntityToLiquibaseXmlMapper : EntityToLiquibaseMapperBase
    {
        public EntityToLiquibaseXmlMapper(string assemblyPath)
            : base(assemblyPath)
        {
            throw new NotImplementedException("Current XML implentation is placeholder and does not work for Liquibase");
        }

        public override string MapEntitiesInNamespace(string rootEntityNamespace)
        {
            var entities = FindEntitiesInNamespace(rootEntityNamespace);
            var createTables = MapCreateTables(entities);

            XmlSerializer xmlSerializer = new(createTables.GetType());
            XmlWriterSettings xmlSettings = new() { Indent = true };

            using StringWriter stringWriter = new();
            using var xmlWriter = XmlWriter.Create(stringWriter, xmlSettings);

            xmlSerializer.Serialize(xmlWriter, createTables);
            var createTableXml = stringWriter.ToString();

            return createTableXml;
        }
    }
}
