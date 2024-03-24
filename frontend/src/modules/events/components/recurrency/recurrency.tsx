import React, { useRef } from 'react';
import { Button } from '@mui/material';
import RecurrencyDialog from './recurrencyDialog';
import useEvent from '../../store/hooks/useEvent';

const Recurrency = () => {
    const AddRecurrencyDialogRef = useRef<any>({});
    const eventStore = useEvent();
    const disabled = eventStore.selectedEvent.uuid ? false : true;

    const handleRecurrency = () => {
        AddRecurrencyDialogRef.current.openDialog();
    }

    return (
        <>
            <Button onClick={handleRecurrency} disabled={disabled}>
                Configure Recurrency
            </Button>
            <RecurrencyDialog ref={AddRecurrencyDialogRef} />
        </>
    )
}

export default Recurrency;