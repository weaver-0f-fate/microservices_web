--changeset your.name:1 labels:example-label context:example-context
--comment: example comment
create table algorithms.person (
    id SERIAL primary key not null,
    name varchar(50) not null,
    address1 varchar(50),
    address2 varchar(50),
    city varchar(30)
)
--rollback DROP TABLE person;