create table algorithms.algorithm (
    uuid UUID DEFAULT gen_random_uuid() PRIMARY KEY NOT NULL,
    name varchar(100) not null,
    audit_dtime TIMESTAMPTZ NOT NULL,
    audit_author VARCHAR(255) NOT NULL,
    is_active BOOLEAN NOT NULL
);

create table algorithms.stage (
    uuid UUID DEFAULT gen_random_uuid() PRIMARY KEY NOT NULL,
    algorithm_uuid UUID NOT NULL,
    name varchar(100) not null,
    audit_dtime TIMESTAMPTZ NOT NULL,
    audit_author VARCHAR(255) NOT NULL,
    is_active BOOLEAN NOT NULL,
    FOREIGN KEY (algorithm_uuid) REFERENCES algorithms.algorithm(uuid) ON DELETE CASCADE
);