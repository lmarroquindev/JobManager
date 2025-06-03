import React from 'react';
import { Component } from 'react';
import type { ReactNode } from 'react';
import { toast } from 'react-toastify';

interface Props {
  children: ReactNode;
}

interface State {
  hasError: boolean;
}

export class ErrorBoundary extends Component<Props, State> {
  state: State = { hasError: false };

  static getDerivedStateFromError() {
    return { hasError: true };
  }

  componentDidCatch(error: Error, errorInfo: React.ErrorInfo) {
    console.error('ErrorBoundary caught an error:', error, errorInfo);
    toast.error(`Ocurrió un error: ${error.message}`);
  }

  render() {
    if (this.state.hasError) {
        return <h2>Something went wrong. Please reload the page.</h2>;
    }

    return this.props.children;
  }
}
