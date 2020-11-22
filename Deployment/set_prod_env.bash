export ConnectionStrings__BookyDatabase="Host=$AZ_HOST;Database=bookydb;Username=$AZ_USER;Password=$AZ_PGPASS;Port=5432;SSLMode=Prefer"

echo "Setting production variables..."
echo "$ConnectionStrings__BookyDatabase"
export DANGER="PROD AZURE "