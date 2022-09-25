CREATE TABLE db_migrations
(
    `id`          BIGINT      NOT NULL AUTO_INCREMENT,
    migration_id  VARCHAR(64) NOT NULL,
    `start_time`  DATETIME    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    checkin_time  DATETIME    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `finish_time` DATETIME    NULL     DEFAULT NULL,
    PRIMARY KEY (`id`),
    UNIQUE INDEX `id_UNIQUE` (`id` ASC),
    UNIQUE INDEX `migration_id_UNIQUE` (migration_id ASC),
    INDEX `checkin_time_INDEX` (checkin_time DESC)
);
