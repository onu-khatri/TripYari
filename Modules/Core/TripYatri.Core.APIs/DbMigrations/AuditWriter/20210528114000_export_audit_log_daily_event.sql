DROP EVENT IF EXISTS export_audit_log_daily;
CREATE EVENT export_audit_log_daily
    ON SCHEDULE
        EVERY 1 DAY
            STARTS CURRENT_DATE + INTERVAL 25 HOUR
    COMMENT 'Exports the daily request audit logs from the TripYatri.Core APIs from yesterday into s3://cds-candidates-dwexports'
    DO
    CALL audit.sp_export_audit_log_daily();
