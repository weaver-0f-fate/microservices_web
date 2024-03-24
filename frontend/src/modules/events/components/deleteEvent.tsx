import React, { forwardRef, useImperativeHandle, useRef, useState } from "react";
import { Button, Dialog, DialogActions, DialogTitle } from "@mui/material";
import useEvent from "../store/hooks/useEvent";
import useDeleteEvent from "../store/hooks/useDeleteEvent";

interface DeleteEventProps {
    closeDetails: () => void;
}

const DeleteEvent = (props: DeleteEventProps) => {
    const { closeDetails } = props;
    const eventStore = useEvent();
    const disabled = eventStore.selectedEvent && eventStore.selectedEvent.uuid ? false : true;
    const confirmDeleteDialog = useRef<any>({});

    const handleDelete = () => {
        confirmDeleteDialog.current.openDialog();
    }

    return (
        <>
        <Button 
            sx={{ ml: 2 }} 
            onClick={handleDelete}
            disabled={disabled}>
            Delete
        </Button>
        <DeleteDialog ref={confirmDeleteDialog} closeDetails={closeDetails}/>
        </>
    )
}

const DeleteDialog = forwardRef((props: any, ref: any) => {
    const { closeDetails } = props;
    const eventStore = useEvent();
    const [openDialog, setOpenDialog] = useState<boolean>(false);
    const deleteEvent = useDeleteEvent();

    useImperativeHandle(ref, () => ({
        openDialog() {
            setOpenDialog(true);
        },
    }))

    const handleDelete = () => {
        if(eventStore.selectedEvent)
            fetch(`https://localhost:7203/api/events/${eventStore.selectedEvent.uuid}`, {
                method: 'DELETE'
            })
            .then(() => {
                if(eventStore.selectedEvent)
                    deleteEvent(eventStore.selectedEvent.uuid);
            })
            setOpenDialog(false)
            closeDetails();
    }

    const handleClose = () => {
        setOpenDialog(false)
    };


    return (
        <Dialog open={openDialog} sx={{ minWidth: 300, minHeight: 50 }}>
            <DialogTitle>Delte event: {eventStore.selectedEvent ? eventStore.selectedEvent.title : ''}</DialogTitle>
            <DialogActions>
                <Button variant="contained" onClick={handleDelete}>
                    Delete
                </Button>
                <Button variant="contained" onClick={handleClose}>
                    Cancel
                </Button>
            </DialogActions>
        </Dialog>
    )
})

export default DeleteEvent;