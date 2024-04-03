import { Grid, InputLabel, TextField } from "@mui/material"
import React, { useEffect, useRef } from 'react';
import { useUpdateEvent } from "../../fetch/useUpdateEvent";
import { useDispatch, useSelector } from "react-redux";
import { SelectedEventState, setSelectedEventImageUrl } from "../../store/selectedEventSlice";


const ImageUrlField = () => {
    const event = useSelector((state: any) => state.selectedEvent as SelectedEventState);
    const dispatch = useDispatch();
    const disabled = event.event.uuid ? false : true;
    const inputRef = useRef<any>();
    const updateEvent = useUpdateEvent();

    useEffect(() => {
        if(event.event.imageUrl)
            inputRef.current.value = event.event.imageUrl;
    })

    const handleImageUrlChange = (e : any) => {
        const value = e.target.value;

        if(value) {
            updateEvent(event.event.uuid, '/imageUrl', value)
            .then(() => {
                dispatch(setSelectedEventImageUrl(value));
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