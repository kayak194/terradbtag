WITH FilteredBusinessObjects(id, name)
AS (SELECT
           BusinessObject.id,
           BusinessObject.name
         FROM BusinessObject, Tag
         WHERE BusinessObject.id = Tag.business_object AND
               Tag.content
               IN ('stadt', 'AtlantischerOzean', 'Island')
         GROUP BY BusinessObject.id
         HAVING COUNT(BusinessObject.id) = 3)
         SELECT id
                 FROM FilteredBusinessObjects
                 LIMIT 30;