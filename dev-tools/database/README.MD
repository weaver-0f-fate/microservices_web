To use Liquibase to make changes to a database, follow these steps:

1. Install Liquibase: Download and install Liquibase from the official website or use a package manager like Maven or Gradle.

2. First go to the Core directory and execute Liquibase commands from step 3 there. Then update the rest databases 1 by 1.

3. Run Liquibase: Execute Liquibase commands to apply the changes to the database. Use the command-line interface or integrate Liquibase into your build process using plugins or scripts. Commands:
2.1. liquibase status
2.2. liquibase update-sql
2.3. liquibase update

4. Verify Changes: After running Liquibase, verify that the changes have been applied correctly by checking the database schema or data.