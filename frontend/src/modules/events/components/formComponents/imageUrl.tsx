import { Grid, InputLabel, TextField } from "@mui/material"
import useEvent from "../../store/hooks/useEvent";
import React, { useEffect, useRef } from 'react';
import useSetImageUrl from "../../store/hooks/eventDetails/useSetImageUrl";
import { useUpdateEvent } from "../../fetch/useUpdateEvent";


const ImageUrlField = () => {
    const eventStore = useEvent();
    const setImageUrl = useSetImageUrl();
    const disabled = eventStore.selectedEvent.uuid ? false : true;
    const inputRef = useRef<any>();
    const updateEvent = useUpdateEvent();

    useEffect(() => {
        if(eventStore.selectedEvent.imageUrl)
            inputRef.current.value = eventStore.selectedEvent.imageUrl;
    })

    const handleImageUrlChange = (e : any) => {
        const value = e.target.value;

        if(value) {
            updateEvent(eventStore.selectedEvent.uuid, '/imageUrl', value)
            .then(() => {
                setImageUrl(value);
            })
        }
    }

    return (
        <Grid item xs={12}>
            <InputLabel htmlFor="imageUrl-input">Image Url</InputLabel>
            <TextField
                id="imageUrl-input"
                fullWidth
                defaultValue={''}
                inputRef={inputRef}
                onChange={handleImageUrlChange}
                disabled={disabled}
            />
        </Grid>
    )
}

export default ImageUrlField;