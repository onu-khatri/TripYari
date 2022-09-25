ALTER TABLE `request_audit_log`
    ADD COLUMN `build_version` VARCHAR(10) NOT NULL DEFAULT '' AFTER `application`;
