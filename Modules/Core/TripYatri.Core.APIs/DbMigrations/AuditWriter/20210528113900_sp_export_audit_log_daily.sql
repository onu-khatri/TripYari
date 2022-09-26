DROP PROCEDURE IF EXISTS sp_export_audit_log_daily;
DELIMITER ///
CREATE PROCEDURE sp_export_audit_log_daily()
BEGIN
    SET @export_sql = CONCAT('SELECT * FROM audit.request_audit_log WHERE start_time BETWEEN SUBDATE(current_date, 1) AND current_date INTO OUTFILE S3 \'s3://cds-candidates-dwexports/$Environment$/audit/request_audit_log/', DATE_FORMAT(SUBDATE(current_date, 1), '%Y%m%d'), '/request_audit_log.tsv\' FIELDS TERMINATED BY \'\\t\' LINES TERMINATED BY \'\\n\' OVERWRITE ON;');
    PREPARE stmt1 FROM @export_sql;
    EXECUTE stmt1;
    DEALLOCATE PREPARE stmt1;

    SET @delete_count = 0;
    delete_loop: LOOP
        SET @delete_count = @delete_count + 1;
        IF @delete_count < 100 THEN
            DELETE FROM audit.request_audit_log
            WHERE start_time < SUBDATE(current_date, 7)
            LIMIT 50000;
            ITERATE delete_loop;
        END IF;
        LEAVE delete_loop;
    END LOOP delete_loop;
END///
