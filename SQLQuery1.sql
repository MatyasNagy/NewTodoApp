
SELECT a.Id, a.FeladatId, a.UserId FROM FeladatKiosztasTable a 
INNER JOIN FeladatTable b ON a.FeladatId = b.Id WHERE b.FeladatCim = 'Mosogatas';