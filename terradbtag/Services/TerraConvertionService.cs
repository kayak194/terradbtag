using System.Collections.Generic;
using System.Xml;
using terradbtag.Framework;

namespace terradbtag.Services
{
    class TerraConvertionService : Service
    {
        protected override bool ServiceAction(object args)
        {
            var connection = new SqliteDatabaseConnection();

            var doc = new XmlDocument();

            var opener = new OpenFileService();
            if (!opener.OpenFile(OpenFileService.XmlFileFilter)) return false;
            doc.Load(opener.SelectedFile);

            var nodes = doc.DocumentElement?.ChildNodes;

            if (nodes == null) return false;

            opener.AcceptNonExistingFiles = true;
            opener.ForceNewFile = true;

            if (!opener.OpenFile(OpenFileService.SqliteDatabaseFilter)) return false;

            connection.Connect(opener.SelectedFile);

            connection.Execute(
                "CREATE TABLE LAND(L_ID TEXT PRIMARY KEY, L_NAME TEXT, EINWOHNER REAL, FLAECHE REAL, HAUPTSTADT, LT_ID TEXT);" +
                "CREATE TABLE STADT(ST_NAME TEXT PRIMARY KEY, L_ID TEXT, LT_ID TEXT, EINWOHNER INTEGER, BREITE REAL, LAENGE REAL);" +
                "CREATE TABLE LANDTEIL(LT_ID TEXT, L_ID TEXT, LT_NAME TEXT, EINWOHNER REAL, LAGE TEXT, HAUPTSTADT TEXT, PRIMARY KEY(L_ID, LT_ID));" +
                "CREATE TABLE KONTINENT(K_NAME TEXT PRIMARY KEY, FLAECHE REAL);" +
                "CREATE TABLE BERG(B_NAME TEXT PRIMARY KEY, GEBIRGE TEXT, HOEHE REAL, JAHR INTEGER, LAENGE REAL, BREITE REAL);" +
                "CREATE TABLE EBENE(E_NAME TEXT PRIMARY KEY, HOEHE REAL, FLAECHE REAL);" +
                "CREATE TABLE FLUSS(F_NAME TEXT PRIMARY KEY, FLUSS TEXT, SEE TEXT, MEER TEXT, LAENGE INTEGER, LAENGEU REAL, BREITEU REAL, LAENGEM REAL, BREITEM REAL);" +
                "CREATE TABLE INSEL(I_NAME TEXT PRIMARY KEY, INSELGRUPPE TEXT, FLAECHE REAL, LAENGE REAL, BREITE REAL);" +
                "CREATE TABLE MEER(M_NAME TEXT PRIMARY KEY, TIEFE REAL);" +
                "CREATE TABLE SEE(S_NAME TEXT PRIMARY KEY, TIEFE REAL, FLAECHE REAL);" +
                "CREATE TABLE WUESTE(W_NAME TEXT PRIMARY KEY, FLAECHE REAL, WUESTENART TEXT);" +
                "CREATE TABLE GEO_BERG(L_ID TEXT, LT_ID TEXT, B_NAME TEXT);" +
                "CREATE TABLE GEO_EBENE(L_ID TEXT, LT_ID TEXT, E_NAME TEXT);" +
                "CREATE TABLE GEO_FLUSS(L_ID TEXT, LT_ID TEXT, F_NAME TEXT);" +
                "CREATE TABLE GEO_INSEL(L_ID TEXT, LT_ID TEXT, I_NAME TEXT);" +
                "CREATE TABLE GEO_MEER(L_ID TEXT, LT_ID TEXT, M_NAME TEXT);" +
                "CREATE TABLE GEO_SEE(L_ID TEXT, LT_ID TEXT, S_NAME TEXT);" +
                "CREATE TABLE GEO_WUESTE(L_ID TEXT, LT_ID TEXT, W_NAME TEXT);" +
                "CREATE TABLE GEHT_UEBER_IN(MEER1 TEXT, MEER2 TEXT);" +
                "CREATE TABLE LIEGT_AN(L_ID TEXT, LT_ID TEXT, ST_NAME TEXT, F_NAME TEXT, S_NAME TEXT, M_NAME TEXT);" +
                "CREATE TABLE UMFASST(L_ID TEXT, K_NAME TEXT, PROZENT REAL);" +
                "CREATE TABLE IST_BENACHBART_ZU(LAND1 TEXT, LAND2 TEXT);" +
                "CREATE TABLE ORGANISATION(O_NAME TEXT PRIMARY KEY, ABKUERZUNG TEXT);" +
                "CREATE TABLE HAT_SITZ_IN(ST_NAME TEXT, LT_ID TEXT, L_ID TEXT, ABKUERZUNG TEXT);" +
                "CREATE TABLE IST_MITGLIED_VON(L_ID TEXT, ABKUERZUNG TEXT, ART TEXT);"
                );

            var i = 1;
            foreach (XmlNode node in nodes)
            {
                if (node == null) return false;
                var table = node.Name;
                var columns = new List<string>();
                var values = new List<string>();

                var attributes = node.Attributes;
                if (attributes == null) return false;
                foreach (XmlAttribute attribute in attributes)
                {
                    columns.Add(attribute.Name);
                    values.Add("\"" + attribute.Value + "\"");
                }

                connection.Execute(
                    $"INSERT INTO {table}({string.Join(", ", columns)}) VALUES ({string.Join(", ", values)});");
                ReportProgress(i++,nodes.Count);
            }

            connection.Disconnect();

            return true;
        }
    }
}
