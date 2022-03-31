Add-Migration "Inicial" -Context RegistroPersonaDbContext


Update-Database -Context  RegistroPersonaDbContext



# Generar script migracion
# Generar script desde la primera migracion hasta la ultima
Script-Migration -Context RegistroPersonaDbContext 0