import { useCallback } from 'react';
import { useSnackbar, VariantType } from 'notistack';

const useNotify = () => {
    const { enqueueSnackbar } = useSnackbar();

    return useCallback((message:string, severity: VariantType) => {
        enqueueSnackbar(message, {
            variant: severity,
            autoHideDuration: 10000,
            preventDuplicate: true,
        });
    }, [enqueueSnackbar]);
}

const useNotifyWithAction = () => {
    const { enqueueSnackbar } = useSnackbar();

    return useCallback((message: string, severity: VariantType, action: any) => {
        enqueueSnackbar(message, {
            variant: severity,
            autoHideDuration: 10000,
            preventDuplicate: true,
            action: action
        });
    }, [enqueueSnackbar]);
}

export { useNotify, useNotifyWithAction };
