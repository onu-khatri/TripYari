CREATE TABLE request_audit_log
(
    `id`           BIGINT        NOT NULL AUTO_INCREMENT,
    `application`  VARCHAR(64)   NOT NULL,
    `request_id`   VARCHAR(128)  NOT NULL,
    `start_time`   DATETIME      NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `elapsed_time` INT           NOT NULL DEFAULT 0,
    `client_ip`    VARCHAR(40)   NULL,
    `owner_id`     VARCHAR(20)   NOT NULL,
    `client_id`    VARCHAR(20)   NOT NULL,
    `account_did`  VARCHAR(20)   NULL,
    `user_did`     VARCHAR(20)   NULL,
    `controller`   VARCHAR(64)   NULL,
    `action`       VARCHAR(64)   NULL,
    `method`       VARCHAR(10)   NOT NULL,
    `path`         VARCHAR(256)  NOT NULL,
    `query`        VARCHAR(256)  NULL,
    `status_code`  INT           NOT NULL,
    `payload`      VARCHAR(4096) NULL,
    PRIMARY KEY (`id`),
    INDEX `start_time_INDEX` (`start_time` ASC)
);
