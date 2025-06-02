import React, { useState, useEffect } from 'react';
import { fetchLogs } from '../services/api';
import { FetchLog } from '../interfaces/FetchLogInterface';
import styles from '../styles/LogsPage.module.css';

const LogsPage: React.FC = () => {
  const [logs, setLogs] = useState<FetchLog[]>([]);
  const [loading, setLoading] = useState(false);

  // On component mount, fetch logs from the API and update the state
  useEffect(() => {
    const loadLogs = async () => {
      try {
        setLoading(true);
        const result = await fetchLogs();
        setLogs(result);
      } catch (error) {
        console.error('Error fetching logs:', error);
      } finally {
        setLoading(false);
      }
    };

    loadLogs();
  }, []);

  return (
    <div className={styles.container}>
      <h1 className={styles.header}>Fetch Logs</h1>
      
      {loading ? (
        <div className={styles.loadingContainer} role="status">
          <div className={styles.loadingSpinner}></div>
        </div>
      ) : (
        <div className={styles.tableContainer}>
          <div className="overflow-x-auto">
            <table className={styles.table}>
              <thead className={styles.tableHeader}>
                <tr>
                  <th className={styles.tableHeaderCell}>City</th>
                  <th className={styles.tableHeaderCell}>Status</th>
                  <th className={styles.tableHeaderCell}>Message</th>
                  <th className={styles.tableHeaderCell}>Timestamp</th>
                </tr>
              </thead>
              <tbody>
                {logs.map((log) => (
                  <tr key={log.id} className={styles.tableRow}>
                    <td className={`${styles.tableCell} ${styles.cityCell}`}>{log.city}</td>
                    <td className={styles.tableCell}>
                      <span className={log.success ? styles.statusSuccess : styles.statusFailed}>
                        {log.success ? 'Success' : 'Failed'}
                      </span>
                    </td>
                    <td className={`${styles.tableCell} ${styles.messageCell}`}>{log.message}</td>
                    <td className={`${styles.tableCell} ${styles.timestampCell}`}>
                      {new Date(log.timestamp).toLocaleString()}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
          
          {logs.length === 0 && !loading && (
            <div className={styles.emptyState}>No logs available</div>
          )}
        </div>
      )}
    </div>
  );
};

export default LogsPage;