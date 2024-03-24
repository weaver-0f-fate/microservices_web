import React from "react";
import { Button, Dialog, DialogActions, DialogContent, DialogTitle } from "@mui/material"
import EventDetails from "./eventDetails"
import { forwardRef, useState, useImperativeHandle } from "react";
import useEvent from "../store/hooks/useEvent";
import DeleteEvent from "./deleteEvent";
import Recurrency from "./recurrency/recurrency";
import Subscribe from "./subscription/subscribe";


const EventDialog = forwardRef((props: any, ref: any) => {
    const eventStore = useEvent();
    const [openDialog, setOpenDialog] = useState<boolean>(false);

    useImperativeHandle(ref, () => ({
        openDialog() {
            setOpenDialog(true);
        },
    }))

    const handleClose = () => {
        setOpenDialog(false);
    }

    return (
        <Dialog open={openDialog}>
            <DialogTitle>{eventStore.selectedEvent.title}</DialogTitle>
            <DialogContent>
                <EventDetails />
            </DialogContent>
            <DialogActions>
                <Subscribe />
                <Recurrency />
                <DeleteEvent closeDetails={handleClose}/>
                <Button onClick={handleClose}>Close</Button>
            </DialogActions>
        </Dialog>
    )
})

export default EventDialog;