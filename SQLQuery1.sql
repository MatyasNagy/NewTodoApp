SELECT FeladatTable.FeladatCim,FeladatTable.FeladatLeiras,FeladatTable.FeladatDate, UserTable.UserName FROM FeladatTable
INNER JOIN FeladatKiosztasTable ON FeladatTable.Id = FeladatKiosztasTable.FeladatId
INNER JOIN UserTable ON FeladatKiosztasTable.UserId = UserTable.Id
WHERE FeladatTable.FeladatCim='Mosas';