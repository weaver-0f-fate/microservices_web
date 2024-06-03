--changeset other.dev:3 labels:example-label context:example-context
--comment: example comment
alter table algorithms.person add column email varchar(40)
--rollback ALTER TABLE person DROP COLUMN country;

