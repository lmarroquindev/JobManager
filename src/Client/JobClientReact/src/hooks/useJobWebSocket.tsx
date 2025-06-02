import { useEffect } from 'react';

interface JobEvent {
  eventType: string;
  jobId: string;
  jobType: string;
  timestamp: string;
}

export function useJobWebSocket(url: string, onEvent: (event: JobEvent) => void) {
  useEffect(() => {
    const socket = new WebSocket(url);

    socket.onmessage = (event) => {
      const data: JobEvent = JSON.parse(event.data);
      onEvent(data);
    };

    return () => {
      socket.close();
    };
  }, [url, onEvent]);
}
