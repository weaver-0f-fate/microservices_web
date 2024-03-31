import { Grid, InputLabel, TextField } from "@mui/material"
import React, { useEffect, useRef } from 'react';
import { useUpdateEvent } from "../../fetch/useUpdateEvent";
import { useDispatch, useSelector } from "react-redux";
import { Event } from '../../../../shared/models/events';
import { setSelectedEventImageUrl } from "../../store/selectedEventSlice";


const ImageUrlField = () => {
    const event = useSelector((state: any) => state.selectedEvent as Event);
    const dispatch = useDispatch();
    const disabled = event.uuid ? false : true;
    const inputRef = useRef<any>();
    const updateEvent = useUpdateEvent();

    useEffect(() => {
        if(event.imageUrl)
            inputRef.current.value = event.imageUrl;
    })

    const handleImageUrlChange = (e : any) => {
        const value = e.target.value;

        if(value) {
            updateEvent(event.uuid, '/imageUrl', value)
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