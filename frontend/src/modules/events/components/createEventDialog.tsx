import React from "react";
import { Button, Container, Dialog, DialogActions, DialogContent, DialogTitle, Grid, InputLabel, TextField } from "@mui/material"
import { forwardRef, useState, useImperativeHandle } from "react";
import { PostEventProps, usePostEvent } from "../fetch/usePostEvent";
import { DateTimePicker } from "@mui/x-date-pickers";
import dayjs from "dayjs";
import { getLocalDate } from "../../../shared/utilities/dateFunctions";
import { useDispatch, useSelector } from "react-redux";
import { Event } from '../../../shared/models/events';
import { addEvent } from "../store/eventsSlice";


const CreateEventDialog = forwardRef((props: any, ref: any) => {
    const [openDialog, setOpenDialog] = useState<boolean>(false);
    const [postEventProps, setPostEventProps] = useState<PostEventProps>({
        category: '',
        title: '',
        place: '',
        date: dayjs(),
        description: '',
        imageUrl: '',
        additionalInfo: '',
    } as PostEventProps);
    const postEvent = usePostEvent();
    const event = useSelector((state: any) => state.selectedEvent as Event);
    const dispatch = useDispatch();

    useImperativeHandle(ref, () => ({
        openDialog() {
            setOpenDialog(true);
        },
    }))

    const handleClose = () => {
        setOpenDialog(false);
    }

    const handleCategoryChange = (e : any) => setPostEventProps({ ...postEventProps, category: e.target.value })
    const handleTitleChange = (e : any) => setPostEventProps({ ...postEventProps, title: e.target.value })
    const handlePlaceChange = (e : any) => setPostEventProps({ ...postEventProps, place: e.target.value })
    const handleDescriptionChange = (e : any) => setPostEventProps({ ...postEventProps, description: e.target.value })
    const handleImageUrlChange = (e : any) => setPostEventProps({ ...postEventProps, imageUrl: e.target.value })
    const handleAdditionalInfoChange = (e : any) => setPostEventProps({ ...postEventProps, additionalInfo: e.target.value })
    const handleDateChange = (e : any) => setPostEventProps({ ...postEventProps, date: e.toDate() })

    const handleCreateEvent = () => {
        postEvent(postEventProps)
            .then(data => {
                dispatch(addEvent(
                    {
                        title: data.title,
                        date: getLocalDate(data.date),
                        uuid: data.uuid,
                        category: data.category,
                        place: data.place
                    }
                
                ));
            })
    }

    return (
        <Dialog open={openDialog}>
            <DialogTitle>{event.title}</DialogTitle>
            <DialogContent>
                <Container maxWidth="sm" sx={{ p: 5 }}>
                    <Grid container spacing={2}>
                        <Grid item xs={12}>
                            <InputLabel htmlFor="category-input">Category</InputLabel>
                            <TextField id="category-input" fullWidth value={postEventProps.category} onChange={handleCategoryChange} />
                        </Grid>
                        <Grid item xs={12}>
                            <InputLabel htmlFor="title-input">Title</InputLabel>
                            <TextField id="title-input" fullWidth value={postEventProps.title} onChange={handleTitleChange} />
                        </Grid>
                        <Grid item xs={12}>
                            <InputLabel htmlFor="place-input">Place</InputLabel>
                            <TextField id="place-input" fullWidth value={postEventProps.place} onChange={handlePlaceChange} />
                        </Grid>
                        <Grid item xs={12}>
                            <InputLabel htmlFor="description-input">Description</InputLabel>
                            <TextField id="description-input" fullWidth value={postEventProps.description} onChange={handleDescriptionChange} />
                        </Grid>
                        <Grid item xs={12}>
                            <InputLabel htmlFor="imageurl-input">Image Url</InputLabel>
                            <TextField id="imageurl-input" fullWidth value={postEventProps.imageUrl} onChange={handleImageUrlChange} />
                        </Grid>
                        <Grid item xs={12}>
                            <InputLabel htmlFor="additionalInfo-input">Additional Info</InputLabel>
                            <TextField id="additionalInfo-input" fullWidth value={postEventProps.additionalInfo} onChange={handleAdditionalInfoChange} />
                        </Grid>
                        <Grid item xs={12}>
                            <InputLabel htmlFor="date-input">Date</InputLabel>
                            <DateTimePicker sx={{ width: '100%' }} value={postEventProps.date} onChange={handleDateChange} />
                        </Grid>
                    </Grid>
                </Container>
            </DialogContent>
            <DialogActions>
                <Button onClick={handleCreateEvent}>Create</Button>
                <Button onClick={handleClose}>Close</Button>
            </DialogActions>
        </Dialog>
    )
})

export default CreateEventDialog;